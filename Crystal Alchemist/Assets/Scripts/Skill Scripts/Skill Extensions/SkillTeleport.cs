using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTeleport : SkillExtension
{   
    private void Start()
    {
        Teleport(this.skill.sender);
    }

    private void Teleport(Character character)
    {
        Player player = this.skill.sender.GetComponent<Player>();

        if (character != null
            && character.isPlayer
            && player != null
            && player.GetComponent<PlayerTeleport>().lastTeleportEnabled())
        {
            character.GetComponent<Player>().GetComponent<PlayerTeleport>().teleportPlayer(true);
        }
    }
}
