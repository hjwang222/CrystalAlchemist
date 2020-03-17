using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum DoorType
{    
    normal,    
    enemy,
    button,
    closed
}

public class Door : Interactable
{
    [Required]
    [BoxGroup("Mandatory")]
    public Animator animator;

    [FoldoutGroup("Tür-Attribute", expanded: false)]
    [EnumToggleButtons]
    [SerializeField]
    private DoorType doorType = DoorType.closed;

    private bool isOpen;

    [FoldoutGroup("Tür-Attribute", expanded: false)]
    [SerializeField]
    private BoxCollider2D boxCollider;

    private new void Start()
    {
        base.Start();

        if (this.isOpen) CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "isOpened", true);
    }

    public override void doOnUpdate()
    {
        if (!this.isPlayerInRange && this.isOpen && this.doorType == DoorType.normal)
        {
            //Normale Tür fällt von alleine wieder zu
            OpenCloseDoor(false, this.context);
        }
    }

    public override void doSomethingOnSubmit()
    {
        if (this.doorType != DoorType.enemy && this.doorType != DoorType.button)
        {
            if (!this.isOpen)           
            {
                 if (this.doorType == DoorType.normal)
                {  
                    if (this.player.GetComponent<PlayerUtils>().canOpenAndUpdateResource(this.price))
                    {
                        //Tür offen!
                        OpenCloseDoor(true, this.context);
                        this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.success);
                    }
                    else
                    {
                        //Tür kann nicht geöffnet werden
                        this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.failed);
                    }
                }
                else
                {
                    //Tür verschlossen
                    this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.failed);
                }
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

        this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.none);
    }

    private void OpenCloseDoor(bool isOpen)
    {
        OpenCloseDoor(isOpen, null);
    }

    private void OpenCloseDoor(bool isOpen, GameObject contextClueChild)
    {
        this.isOpen = isOpen;
        CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "isOpened", this.isOpen);
        this.boxCollider.enabled = !this.isOpen;

        if (contextClueChild != null)
        {
            //Wenn Spieler in Reichweite ist und Tür zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.isOpen) contextClueChild.SetActive(false);
            else if (!this.isOpen && this.isPlayerInRange) contextClueChild.SetActive(true);
            else contextClueChild.SetActive(false);
        }

        CustomUtilities.Audio.playSoundEffect(this.gameObject, this.soundEffect);
    }
}
