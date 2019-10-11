using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRequirements : PreLoadModule
{
    public override void checkRequirements()
    {
        Player player = this.skill.sender.GetComponent<Player>();

        if (player != null)
        {
            bool teleportEnabled = player.GetComponent<PlayerTeleport>().getLastTeleport();
            
            if (!teleportEnabled) this.skill.basicRequirementsExists = false;
            else this.skill.basicRequirementsExists = true;            
        }
    }
}
