using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;
using Sirenix.OdinInspector;

#region Enums
public enum UIType
{
    resource,           
    buffsOnly,          //Nur Buffs
    debuffsOnly,        //Nur Debuffs
    BuffsAndDebuffs,    //Buffs und Debuffs
    DebuffsAndBuffs     //Debuffs und Buffs
}
#endregion

public class StatusBar : MonoBehaviour
{
    #region Attribute
    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

    [BoxGroup("UI Typ")]
    [SerializeField]
    private UIType UIType = UIType.resource;

    [BoxGroup("UI Typ")]
    [ShowIf("UIType", UIType.resource)]
    [SerializeField]
    private ResourceType resourceType;

    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    [SerializeField]
    private Sprite full;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    [SerializeField]
    private Sprite quarterhalf;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    [SerializeField]
    private Sprite half;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    [SerializeField]
    private Sprite quarter;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    [SerializeField]
    private Sprite empty;
    [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
    [SerializeField]
    private List<GameObject> icons = new List<GameObject>();

    [FoldoutGroup("Warning", expanded: false)]
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    [FoldoutGroup("Warning", expanded: false)]
    private AudioClip lowSoundEffect;
    [SerializeField]
    [FoldoutGroup("Warning", expanded: false)]
    private float schwelle = 0.5f;
    [FoldoutGroup("Warning", expanded: false)]
    [SerializeField]
    private float audioInterval = 1.5f;

    private AudioSource audioSource;
    private bool playLow;
    private float elapsed;
    private Player player;

    private float maxValue;
    private float currentValue;
    #endregion


    #region Start Funktionen (Awake, Init, SetValues)

    void Start()
    {
        init();
    }

    private void init()
    {
        this.player = this.playerStats.player;
        this.audioSource = GetComponent<AudioSource>();
        setStatusBar();

        if(this.UIType == UIType.resource) UpdateGUIHealthMana();
    }

    private void LateUpdate()
    {
        if (this.playLow && this.player.currentState != CharacterState.dead)
        {
            if (elapsed <= 0)
            {
                elapsed = audioInterval;
                CustomUtilities.Audio.playSoundEffect(this.lowSoundEffect);
            }
            else
            {
                elapsed -= Time.deltaTime;
            }
        }
    }

    private void setValues()
    {
        if(this.resourceType == ResourceType.life)
        {
            this.maxValue = this.player.maxLife;
            this.currentValue = this.player.life;
        }
        else
        {
            this.maxValue = this.player.maxMana;
            this.currentValue = this.player.mana;
        }
    }

    private void setStatusBar()
    {
        setValues();

        for (int i = 0; i < this.icons.Count; i++)
        {
            this.icons[i].SetActive(false);
            if (i + 1 <= this.maxValue) this.icons[i].SetActive(true);
        }
    }
    #endregion


    #region Update Signal Funktionen (Life, Mana, StatusEffects)    
    public void UpdateGUIHealthMana()
    {
        setStatusBar();

        Sprite sprite = null;

        for (int i = 0; i < (int)this.maxValue; i++)
        {
            if (i <= this.currentValue - 1)
            {                
                sprite = this.full;
            }
            else if (i <= this.currentValue - 0.75f)
            {
                sprite = this.quarterhalf;
            }
            else if (i <= this.currentValue - 0.5f)
            {
                sprite = this.half;
            }
            else if (i <= this.currentValue - 0.25f)
            {
                sprite = this.quarter;
            }
            else 
            {
                sprite = this.empty;
            }

            try
            {
                Transform temp = this.gameObject.transform.GetChild(i);
                temp.GetComponent<Image>().sprite = sprite;
            }
            catch(Exception ex)
            {
                string temp = ex.ToString();
            }
            
        }

        if (this.currentValue <= this.schwelle && this.player.currentState != CharacterState.dead)
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
