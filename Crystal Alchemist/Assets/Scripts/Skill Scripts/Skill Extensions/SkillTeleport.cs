using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTeleport : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    [SerializeField]
    private bool showAnimation = false;

    private void Start()
    {
        Teleport(this.skill.sender);
    }

    private void Teleport(Character character)
    {
        Player player = this.skill.sender.GetComponent<Player>();

        string scene;
        Vector2 position;

        if (character != null
            && character.isPlayer
            && player != null
            && player.GetComponent<PlayerTeleport>().getLastTeleport(out scene, out position))
        {
            character.GetComponent<Player>().GetComponent<PlayerTeleport>().teleportPlayer(scene, position, showAnimation);
        }
    }
}
