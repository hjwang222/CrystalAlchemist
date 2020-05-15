using UnityEngine;
using Sirenix.OdinInspector;

public class Savepoint : Interactable
{
    [BoxGroup("SavePoint")]
    [Tooltip("Teleport Info of this savepoint")]
    [SerializeField]
    private TeleportStats teleportPoint; 

    [BoxGroup("Player")]
    [Tooltip("To add this Teleport Point to quicktravel")]
    [SerializeField]
    private PlayerTeleportList teleportList; 

    [BoxGroup("UI")]
    [Tooltip("To store info for UI (Respawn)")]
    [SerializeField]
    private TeleportStats savePointInfo;

    public override void doSomethingOnSubmit()
    {
        this.player.updateResource(CostType.life, this.player.stats.maxLife);
        this.player.updateResource(CostType.mana, this.player.stats.maxMana);

        this.teleportList.AddTeleport(this.teleportPoint); //add to teleport list    
        this.savePointInfo.SetValue(this.teleportPoint); //set for UI (Respawn)

        MenuEvents.current.OpenSavepoint();
    }
}
