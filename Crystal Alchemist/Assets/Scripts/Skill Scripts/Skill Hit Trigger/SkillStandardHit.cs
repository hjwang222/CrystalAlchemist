using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillStandardHit : SkillHitTrigger
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    public virtual void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
    }

    public virtual void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
    }
}
