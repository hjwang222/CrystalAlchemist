﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnalyseUI : MonoBehaviour
{
    private GameObject target;

    [Header("Easy Access Objects")]
    [SerializeField]
    private TextMeshPro TMPcharacterName;
    [SerializeField]
    private SpriteFillBar lifeBar;
    [SerializeField]
    private SpriteRenderer ImageitemPreview;
    [SerializeField]
    private SpriteRenderer ImageObjectitemPreview;

    [Header("Gruppen")]
    [SerializeField]
    private GameObject enemyInfo;
    [SerializeField]
    private GameObject objectInfo;
    [SerializeField]
    private StatusEffectBar statusEffectBar;

    private Character character;
    private Rewardable rewardableObject;
    private List<StatusEffectUI> activeStatusEffectUIs = new List<StatusEffectUI>();
    private List<StatusEffectGameObject> activeStatusEffects = new List<StatusEffectGameObject>();

    private void Start()
    {
        init();
    }

    public void setTarget(GameObject target)
    {
        this.enemyInfo.SetActive(false);
        this.objectInfo.SetActive(false);
        //this.aggrobar.gameObject.SetActive(false);
        this.target = target;
    }

    private void init()
    {
        //set type of Analyse
        this.transform.position = new Vector2(this.target.transform.position.x, this.target.transform.position.y);

        if (this.target.GetComponent<Interactable>() != null)
        {
            //set UI to Treasure/Object Info
            this.rewardableObject = this.target.GetComponent<Rewardable>();
            this.objectInfo.SetActive(true);
        }
        else if (this.target.GetComponent<Character>() != null)
        {
            //set UI to Character Info
            this.character = this.target.GetComponent<Character>();

            if (this.character.stats.characterType != CharacterType.Object)
            {
                //show Basic Information for Enemies
                this.enemyInfo.SetActive(true);
                this.statusEffectBar.setCharacter(character);
            }
            else this.objectInfo.SetActive(true);

            AI enemy = this.character.GetComponent<AI>();
            /*if (enemy != null)
            {
                this.aggrobar.gameObject.SetActive(true);
                this.aggrobar.setEnemy(enemy);
            }*/
        }
    }

    private void LateUpdate()
    {
        updateInfos();
    }

    private void updateInfos()
    {
        if (this.character != null)
        {
            if (this.character.stats.characterType != CharacterType.Object)
            {
                showEnemyInfo();                
            }
            else showItemInfo();
        }
        else if (this.rewardableObject != null)
        {
            showItemInfo();
        }
    }

    private void showEnemyInfo()
    {
        this.ImageitemPreview.gameObject.SetActive(false);

        this.TMPcharacterName.text = FormatUtil.getLanguageDialogText(this.character.stats.characterName, this.character.stats.englischCharacterName);
        this.lifeBar.fillAmount((this.character.life / this.character.maxLife));

        if (this.character.itemDrop != null && this.character.currentState != CharacterState.dead)
        {
            this.ImageitemPreview.gameObject.SetActive(true);
            this.ImageitemPreview.sprite = this.character.itemDrop.stats.getSprite(); //TODONEW
        }
    }

    private void showItemInfo()
    {
        if (this.rewardableObject != null)
        {
            //Show Object Information
            if (rewardableObject.itemDrop != null && this.rewardableObject.currentState != objectState.opened)
            {
                this.ImageObjectitemPreview.sprite = rewardableObject.itemDrop.stats.getSprite();
            }
            else
            {
                Destroy(this.gameObject);
            }           
        }
        else if (this.character != null)
        {
            //Show Object Information
            if (this.character.itemDrop != null && this.character.currentState != CharacterState.dead)
            {
                this.ImageObjectitemPreview.sprite = this.character.itemDrop.stats.getSprite();
            }            
        }
    }

    private void destroyIt()
    {
        //Wenn Truhe geöffnet wurde oder Gegner tot ist
        Destroy(this);
    }
}
