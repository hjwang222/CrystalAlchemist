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

public class Treasure : Interactable
{
    #region Attribute   

    [FoldoutGroup("Treasure Options", expanded: false)]
    [Required]
    public Animator anim;

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

    private void Start()
    {
        base.Start();

        Utilities.Format.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);
    }

    #endregion


    #region Update Funktion

    public override void doOnUpdate()
    {
        //Close when opened
        if (this.currentState == objectState.opened && this.player != null && this.player.currentState == CharacterState.interact)
        {           
            //Entferne Item aus der Welt und leere die Liste
            foreach (Item item in this.items)
            {
                Destroy(item.gameObject);
            }
            this.items.Clear();

            if (this.treasureType == TreasureType.lootbox)
            {
                Utilities.UnityUtils.SetAnimatorParameter(this.anim, "isOpened", false);
                this.currentState = objectState.normal;
                Utilities.Items.setItem(this.lootTable, this.multiLoot, this.items);
            }
        }
        /*
        if (this.context != null)
        {
            //Wenn Spieler in Reichweite ist und Truhe zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.currentState == objectState.opened) this.context.SetActive(false);
            else if (this.currentState != objectState.opened && this.isPlayerInRange) this.context.SetActive(true);
            else this.context.SetActive(false);
        }*/
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

    private void OpenChest()
    {        
        Utilities.UnityUtils.SetAnimatorParameter(this.anim, "isOpened", true);
        this.currentState = objectState.opened;

        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);

        if (this.soundEffect != null && this.items.Count > 0)
        {
            //Spiele Soundeffekte ab
            Utilities.Audio.playSoundEffect(this.audioSource, this.soundEffect);
            Utilities.Audio.playSoundEffect(this.audioSource, this.soundEffectTreasure);

            //Zeige Item
            this.showTreasureItem();

            //OLD, muss besser gehen!
            //Gebe Item dem Spieler
            foreach (Item item in this.items)
            {
                this.player.collect(item, false);
                text = Utilities.Format.getDialogBoxText("Du hast", item.amount, item, "erhalten!");

                if (GlobalValues.useAlternativeLanguage) text = Utilities.Format.getDialogBoxText("You obtained", item.amount, item, "!");
            }                
        }
        else
        {
            //Kein Item drin
            text = "Die Kiste ist leer... .";
            if (GlobalValues.useAlternativeLanguage) text = "This chest is empty... .";
        }

        if (this.player != null) this.player.showDialogBox(text);
    }


    private void canOpenChest()
    {
        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);
        if (Utilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price, text)) OpenChest();
    }

    public void showTreasureItem()
    {
        for (int i = 0; i < this.items.Count; i++)
        {
            //Item instanziieren und der Liste zurück geben und das Item anzeigen
            this.items[i] = Instantiate(this.items[i], this.transform.position, Quaternion.identity, this.transform);
            this.items[i].GetComponent<Item>().showFromTreasure();
        }
    }

    #endregion
}
