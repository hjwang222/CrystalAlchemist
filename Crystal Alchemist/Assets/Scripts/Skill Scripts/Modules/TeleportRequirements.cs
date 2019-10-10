using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRequirements : PreLoadModule
{
    [SerializeField]
    private BuffSkill skill;

    public override void checkRequirements()
    {
        Player player = this.skill.sender.GetComponent<Player>();

        if (player != null)
        {
            bool teleportEnabled = player.GetComponent<PlayerTeleport>().getLastTeleport();
            if (this.skill.supportType == SupportType.teleport)
            {
                if (!teleportEnabled) this.skill.basicRequirementsExists = false;
                else this.skill.basicRequirementsExists = true;
            }
        }
    }
}
