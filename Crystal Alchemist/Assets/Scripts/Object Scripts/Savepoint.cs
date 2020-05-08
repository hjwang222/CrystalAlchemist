using UnityEngine;
using Sirenix.OdinInspector;

public class Savepoint : Interactable
{
    [BoxGroup("SavePoint")]
    [SerializeField]
    private SimpleSignal openSaveMenu;

    [BoxGroup("SavePoint")]
    [SerializeField]
    private TeleportStats teleportPoint;

    [BoxGroup("Player")]
    [SerializeField]
    private PlayerTeleportList teleportList;

    [BoxGroup("Player")]
    [SerializeField]
    private TeleportStats playerTeleport;

    public override void doSomethingOnSubmit()
    {
        this.teleportList.AddTeleport(this.teleportPoint);
        this.playerTeleport.SetValue(this.teleportPoint);

        openSaveMenu.Raise();        
    }
}
