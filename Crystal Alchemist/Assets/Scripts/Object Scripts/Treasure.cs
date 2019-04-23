using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Treasure : Interactable
{
    #region Attribute   

    [Header("Truhen-Attribute")]
    public AudioClip soundEffectTreasure;

    #endregion


    #region Start Funktionen

    private void Start()
    {
        init();        
    }

    #endregion


    #region Update Funktion

    private void Update()
    {
        if (this.isPlayerInRange && this.currentState != objectState.opened && Input.GetButtonDown("A-Button"))
        {
            OpenChest();           
        }
        else if (this.isPlayerInRange && Input.GetButtonDown("A-Button"))
        {   
            //Entferne Item aus der Welt und leere die Liste
            foreach (GameObject item in this.items)
            {
                Destroy(item);
            }
            this.items.Clear();
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
        Utilities.SetParameter(this.animator, "isOpened", true);
        this.currentState = objectState.opened;

        if (this.soundEffect != null && this.items.Count > 0)
        {
            //Spiele Soundeffekte ab
            Utilities.playSoundEffect(this.audioSource, this.soundEffect);
            Utilities.playSoundEffect(this.audioSource, this.soundEffectTreasure);

            //Zeige Item
            this.showTreasureItem();

            //OLD, muss besser gehen!
            //Gebe Item dem Spieler
            foreach (GameObject item in this.items) item.GetComponent<Item>().collect(this.character.GetComponent<Player>(), false);
        }
        else
        {
            //Kein Item drin
            this.text = "Die Kiste ist leer... .";
        }

        if (this.character.GetComponent<Player>() != null) this.character.GetComponent<Player>().showDialogBox(this.text);
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
