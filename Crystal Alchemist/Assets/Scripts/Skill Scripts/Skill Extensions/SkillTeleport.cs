using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTeleport : SkillExtension
{
    [BoxGroup]
    [Required]
    [SerializeField]
    private TeleportStats lastTeleport;

    private void Start() => Teleport(this.skill.sender);    

    private void Teleport(Character character)
    {
        Player player = this.skill.sender.GetComponent<Player>();
        if (player != null) player.GetComponent<PlayerTeleport>().SwitchScene();        
    }
}
