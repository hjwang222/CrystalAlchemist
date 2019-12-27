using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillStandardHit : SkillHitTrigger
{
    public virtual void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        if (CustomUtilities.Collisions.checkCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
    }

    public virtual void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (CustomUtilities.Collisions.checkCollision(hittedCharacter, this.skill)) this.skill.hitIt(hittedCharacter);
    }
}
