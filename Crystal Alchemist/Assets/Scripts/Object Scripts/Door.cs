using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{    
    normal,
    key,
    enemy,
    button,
    bosskey,
    closed
}

public class Door : Interactable
{
    
    [Header("Tür-Attribute")]
    public DoorType doorType = DoorType.closed;
    private bool isOpen;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        init();
        this.boxCollider = GetComponent<BoxCollider2D>();

        if (this.isOpen) Utilities.SetParameter(this.animator, "isOpened", true);
    }

    private void Update()
    {
        if (this.doorType != DoorType.enemy && this.doorType != DoorType.button)
        {
            if (this.isPlayerInRange && !this.isOpen && Input.GetButtonDown("A-Button"))
            {
                if (this.doorType == DoorType.key)
                {

                    //check if Player has keys
                    //if so, OpenDoor();
                    //reduce keys by 1
                    //ansonsten zeige Nachricht an
                }
                else if (this.doorType == DoorType.bosskey)
                {                    
                    //check if Player had specific boss key
                    //if so, OpenDoor();
                }
                else if (this.doorType == DoorType.normal)
                {
                    //Normale Tür, einfach aufmachen
                    OpenCloseDoor(true, this.context);
                }
                else
                {
                    if (this.character.GetComponent<Player>() != null) this.character.GetComponent<Player>().showDialogBox(this.text);
                }
            }
            else if (!this.isPlayerInRange && this.isOpen && this.doorType == DoorType.normal)
            {
                //Normale Tür fällt von alleine wieder zu
                OpenCloseDoor(false, this.context);
            }            
        }
        else if (this.doorType == DoorType.enemy)
        {
            //Wenn alle Feinde tot sind, OpenDoor()
        }
        else if (this.doorType == DoorType.button)
        {
            //Wenn Knopf gedrückt wurde, OpenDoor()
        }
    }

    private void OpenCloseDoor(bool isOpen)
    {
        OpenCloseDoor(isOpen, null);
    }

    private void OpenCloseDoor(bool isOpen, GameObject contextClueChild)
    {
        this.isOpen = isOpen;
        Utilities.SetParameter(this.animator, "isOpened", this.isOpen);
        this.boxCollider.enabled = !this.isOpen;

        if (contextClueChild != null)
        {
            //Wenn Spieler in Reichweite ist und Tür zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.isOpen) contextClueChild.SetActive(false);
            else if (!this.isOpen && this.isPlayerInRange) contextClueChild.SetActive(true);
            else contextClueChild.SetActive(false);
        }

        Utilities.playSoundEffect(this.audioSource, this.soundEffect);

    }
}
