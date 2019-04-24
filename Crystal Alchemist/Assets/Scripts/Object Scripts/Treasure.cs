using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TreasureType
{
    normal,
    lootbox
}

public class Treasure : Interactable
{
    #region Attribute   

    [Header("Truhen-Attribute")]
    public AudioClip soundEffectTreasure;
    public TreasureType treasureType = TreasureType.normal;

    [Header("Text-Attribute")]
    public TextMeshPro priceText;
    public Color fontColor;
    public Color outlineColor;
    public float outlineWidth = 0.25f;
    public Animator anim;
    #endregion


    #region Start Funktionen

    private void Start()
    {
        init();

        Utilities.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);
    }

    #endregion


    #region Update Funktion

    private void Update()
    {
        if (this.isPlayerInRange && this.currentState != objectState.opened && Input.GetButtonDown("Submit"))
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
        else if (this.isPlayerInRange && this.currentState == objectState.opened && Input.GetButtonDown("Submit"))
        {   
            //Entferne Item aus der Welt und leere die Liste
            foreach (Item item in this.items)
            {
                Destroy(item.gameObject);
            }
            this.items.Clear();

            if (this.treasureType == TreasureType.lootbox)
            {
                Utilities.SetParameter(this.anim, "isOpened", false);
                this.currentState = objectState.normal;
                Utilities.setItem(this.lootTable, this.multiLoot, this.items);
            }
        }   

        if (this.context != null)
        {
            //Wenn Spieler in Reichweite ist und Truhe zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.currentState == objectState.opened) this.context.SetActive(false);
            else if (this.currentState != objectState.opened && this.isPlayerInRange) this.context.SetActive(true);
            else this.context.SetActive(false);
        }
    }

    #endregion


    #region Treasure Chest Funktionen (open, show Item)

    private void OpenChest()
    {        
        Utilities.SetParameter(this.anim, "isOpened", true);
        this.currentState = objectState.opened;

        string text = this.text;

        if (this.soundEffect != null && this.items.Count > 0)
        {
            //Spiele Soundeffekte ab
            Utilities.playSoundEffect(this.audioSource, this.soundEffect);
            Utilities.playSoundEffect(this.audioSource, this.soundEffectTreasure);

            //Zeige Item
            this.showTreasureItem();

            //OLD, muss besser gehen!
            //Gebe Item dem Spieler
            foreach (Item item in this.items)
            {
                this.player.collect(item, false);
                text = text.Replace("%XY%", item.itemName);
            }
        }
        else
        {
            //Kein Item drin
            text = "Die Kiste ist leer... .";
        }

        if (this.player != null) this.player.showDialogBox(text);
    }


    private void canOpenChest()
    {
        if (Utilities.canOpen(this.currencyNeeded, this.player, this.price)) OpenChest();
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
