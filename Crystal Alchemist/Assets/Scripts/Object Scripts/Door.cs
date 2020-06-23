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
    [SerializeField]
    private DoorType doorType = DoorType.closed;

    private bool isOpen;

    private new void Start()
    {
        base.Start();

        if (this.isOpen) AnimatorUtil.SetAnimatorParameter(this.animator, "Close");
    }

    public override void DoOnUpdate()
    {
        if (!this.isPlayerInRange && this.isOpen && this.doorType == DoorType.normal)
        {
            //Normale Tür fällt von alleine wieder zu
            OpenCloseDoor(false, this.context);
        }
    }

    public override void DoOnSubmit()
    {
        if (this.doorType != DoorType.enemy && this.doorType != DoorType.button)
        {
            if (!this.isOpen)           
            {
                 if (this.doorType == DoorType.normal)
                {  
                    if (this.player.canUseIt(this.costs))
                    {
                        //Tür offen!
                        this.player.reduceResource(this.costs);
                        OpenCloseDoor(true, this.context);
                        ShowDialog(DialogTextTrigger.success);
                    }
                    else
                    {
                        //Tür kann nicht geöffnet werden
                        ShowDialog(DialogTextTrigger.failed);
                    }
                }
                else
                {
                    //Tür verschlossen
                    ShowDialog(DialogTextTrigger.failed);
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

        ShowDialog(DialogTextTrigger.none);
    }

    private void OpenCloseDoor(bool isOpen)
    {
        OpenCloseDoor(isOpen, null);
    }

    private void OpenCloseDoor(bool isOpen, ContextClue contextClueChild)
    {
        this.isOpen = isOpen;

        if (this.isOpen) AnimatorUtil.SetAnimatorParameter(this.animator, "Open");
        else AnimatorUtil.SetAnimatorParameter(this.animator, "Close");

        if (contextClueChild != null)
        {
            //Wenn Spieler in Reichweite ist und Tür zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.isOpen) contextClueChild.gameObject.SetActive(false);
            else if (!this.isOpen && this.isPlayerInRange) contextClueChild.gameObject.SetActive(true);
            else contextClueChild.gameObject.SetActive(false);
        }
    }
}
