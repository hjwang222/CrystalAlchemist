using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTeleport : SkillExtension
{
    [Required]
    [SerializeField]
    private PlayerTeleportList playerTeleport;

    public override void Initialize()
    {
        this.playerTeleport.SetReturnTeleport();
        GameEvents.current.DoTeleport();
    }
}