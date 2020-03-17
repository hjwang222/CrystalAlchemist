using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public enum TreasureType
{
    normal,
    lootbox
}

public class Treasure : Rewardable
{
    #region Attribute   

    [FoldoutGroup("Treasure Options", expanded: false)]
    [Required]
    public Animator anim;

    [BoxGroup("Mandatory")]
    [Required]
    public GameObject showItem;

    [FoldoutGroup("Treasure Options", expanded: false)]
    public AudioClip soundEffectTreasure;

    [FoldoutGroup("Treasure Options", expanded: false)]
    [EnumToggleButtons]
    public TreasureType treasureType = TreasureType.normal;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public TextMeshPro priceText;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public Color fontColor;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public Color outlineColor;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public float outlineWidth = 0.25f;
    #endregion


    #region Start Funktionen

    public override void Start()
    {
        base.Start();
        CustomUtilities.Format.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);

        if (this.inventory.Count == 0 && this.treasureType == TreasureType.normal) changeTreasureState(true);        
    }

    #endregion


    #region Update Funktion

    public override void doOnUpdate()
    {
        //Close when opened
        if (this.currentState == objectState.opened && this.player != null && this.player.currentState == CharacterState.interact)
        {          
            this.inventory.Clear();

            if (this.treasureType == TreasureType.lootbox)
            {
                changeTreasureState(false);
                this.setLoot();
            }

            //Verstecke gezeigtes Item wieder
            for(int i = 0; i < this.showItem.transform.childCount; i++)
            {
                Destroy(this.showItem.transform.GetChild(i).gameObject);
            }
            this.showItem.SetActive(false);
        }
    }

    public override void doSomethingOnSubmit()
    {        
        if (this.currentState != objectState.opened)
        {
            if (this.treasureType == TreasureType.normal)
            {
                canOpenChest();
            }
            else if (this.treasureType == TreasureType.lootbox)
            {
                canOpenChest();
            }
        }              
    }

    #endregion


    #region Treasure Chest Funktionen (open, show Item)

    private void changeTreasureState(bool openChest)
    {
        if (openChest)
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.anim, "isOpened", true);
            this.currentState = objectState.opened;
            this.context.SetActive(false);
        }
        else
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.anim, "isOpened", false);
            this.currentState = objectState.normal;
            this.context.SetActive(true);
        }
    }

    private void OpenChest()
    {
        changeTreasureState(true);
        CustomUtilities.Audio.playSoundEffect(this.gameObject, this.soundEffect);

        if (this.soundEffect != null && this.inventory.Count > 0)
        {
            //Spiele Soundeffekte ab            
            CustomUtilities.Audio.playSoundEffect(this.gameObject, this.soundEffectTreasure, GlobalValues.backgroundMusicVolume);

            //Zeige Item
            this.showTreasureItem();

            //Gebe Item dem Spieler
            foreach (ItemDrop item in this.inventory)
            {
                this.player.GetComponent<PlayerUtils>().CollectItem(item.stats);
                this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.success, item.stats);
            }                
        }
        else
        {
            //Kein Item drin
            this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.empty);
        }
    }


    private void canOpenChest()
    {
        if (this.player.GetComponent<PlayerUtils>().canOpenAndUpdateResource(this.price)) OpenChest();
        else this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.failed);
    }

    public void showTreasureItem()
    {
        for (int i = 0; i < this.inventory.Count; i++)
        {
            //Item instanziieren und der Liste zurück geben und das Item anzeigen            
            this.showItem.SetActive(true);

            Collectable collectable = this.inventory[i].Instantiate(this.showItem.transform.position);
            collectable.SetAsTreasureItem(this.showItem.transform);
        }
    }

    #endregion
}
