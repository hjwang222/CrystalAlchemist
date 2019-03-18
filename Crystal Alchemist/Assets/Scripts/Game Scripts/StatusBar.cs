using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

#region Enums
public enum UIType
{
    health,             //Life-Anzeige
    mana,               //Mana-Anzeige
    buffsOnly,          //Nur Buffs
    debuffsOnly,        //Nur Debuffs
    BuffsAndDebuffs,    //Buffs und Debuffs
    DebuffsAndBuffs     //Debuffs und Buffs
}
#endregion

public class StatusBar : MonoBehaviour
{
    #region Attribute
    [Header("Ziel der Anzeige (Spieler)")]
    public Character character;

    [Header("Art der GUI")]
    public UIType type = UIType.health;

    [Header("Sprites für Mana und Leben")]
    public Sprite full;
    public Sprite quarterhalf;
    public Sprite half;
    public Sprite quarter;
    public Sprite empty;
    public GameObject icon;

    public AudioClip lowSoundEffect;
    public float audioInterval = 1.5f;
    public GameObject warning;
    public FloatValue soundEffectValue;

    private AudioSource audioSource;
    private float maximum;
    private float current;
    private bool playLow;
    private float elapsed;
    #endregion


    #region Start Funktionen (Awake, Init, SetValues)

    void Awake()
    {
        init();
    }

    private void init()
    {
        setValues();

        for (int i = 0; i < (int)this.maximum-1; i++)
        {
            setStatusBar();            
        }

        UpdateGUIHealthMana();
    }

    private void LateUpdate()
    {
        if (playLow)
        {
            if (elapsed <= 0)
            {
                elapsed = audioInterval;
                Utilities.playSoundEffect(this.audioSource, this.lowSoundEffect, this.soundEffectValue);
            }
            else
            {
                elapsed -= Time.deltaTime;
            }
        }
    }

    private void setValues()
    {
        this.audioSource = GetComponent<AudioSource>();

        if (this.type == UIType.mana)
        {
            this.maximum = this.character.attributeMaxMana;
            this.current = this.character.mana;
        }
        else if (this.type == UIType.health)
        {
            this.maximum = this.character.attributeMaxLife;
            this.current = this.character.life;
        }
    }

    private GameObject setStatusBar()
    {
        GameObject temp;

        temp = (GameObject)GameObject.Instantiate(icon);
        temp.transform.SetParent(this.gameObject.transform);
        temp.SetActive(true);
        temp.hideFlags = HideFlags.HideInHierarchy;

        return temp;
    }
    #endregion


    #region Update Signal Funktionen (Life, Mana, StatusEffects)
    public void UpdateGUIStatusEffects()
    {        
        for (int i = 1; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).gameObject.activeInHierarchy) Destroy(this.transform.GetChild(i).gameObject);
        }

        //füge ggf. beide Listen hinzu oder selektiere nur eine
        List<StatusEffect> effectList = new List<StatusEffect>();
        if (this.type == UIType.buffsOnly) effectList = this.character.buffs;
        else if (this.type == UIType.debuffsOnly) effectList = this.character.debuffs;
        else if (this.type == UIType.BuffsAndDebuffs)
        {
            effectList.AddRange(this.character.buffs);
            effectList.AddRange(this.character.debuffs);
        }
        else if (this.type == UIType.DebuffsAndBuffs)
        {
            effectList.AddRange(this.character.debuffs);
            effectList.AddRange(this.character.buffs);
        }

        for (int i = 0; i < effectList.Count; i++)
        {
            //Make a copy
            GameObject statusEffectGUI = Instantiate(this.transform.GetChild(0), this.transform).gameObject;

            StatusEffect statusEffectFromList = effectList[i];
            statusEffectGUI.GetComponent<Image>().sprite = statusEffectFromList.iconSprite;

            string seconds = Utilities.setDurationToString(statusEffectFromList.statusEffectTimeLeft)+"s";
            if (statusEffectFromList.endType == StatusEffectEndType.mana) seconds = "";

            statusEffectGUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = seconds;

            statusEffectGUI.SetActive(true);
            statusEffectGUI.hideFlags = HideFlags.HideInHierarchy;
        }
    }
    
    public void UpdateGUIHealthMana()
    {
        setValues();

        Sprite sprite = null;

        for (int i = 0; i < (int)this.maximum; i++)
        {
            if (i <= this.current - 1)
            {                
                sprite = this.full;
            }
            else if (i <= this.current - 0.75f)
            {
                sprite = this.quarterhalf;
            }
            else if (i <= this.current - 0.5f)
            {
                sprite = this.half;
            }
            else if (i <= this.current - 0.25f)
            {
                sprite = this.quarter;
            }
            else 
            {
                sprite = this.empty;
            }
            
            Transform temp = this.gameObject.transform.GetChild(i);
            temp.GetComponent<Image>().sprite = sprite;
        }

        if (this.current <= 0.5f)
        {
            this.playLow = true;
            if (this.warning != null) this.warning.SetActive(true);
        }
        else 
        {
            this.playLow = false;
            if (this.warning != null) this.warning.SetActive(false);
        }
        
    }
    #endregion
}
