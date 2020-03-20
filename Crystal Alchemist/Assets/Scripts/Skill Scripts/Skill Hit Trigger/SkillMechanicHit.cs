using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum hitType
{
    onDestroyAll,
    onDestroyTarget,
    onTrigger
}

public class SkillMechanicHit : SkillHitTrigger
{
    [HideInInspector]
    public float percentage = 100;

    [BoxGroup("Mechanics")]
    [SerializeField]
    [EnumToggleButtons]
    private hitType hitType;

    private void OnDestroy()
    {
        if (this.hitType == hitType.onDestroyAll) hitAllCharacters();
        else if (this.hitType == hitType.onDestroyTarget) hitTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.hitType == hitType.onTrigger 
            && CollisionUtil.checkCollision(collision, this.skill))
        {
            hitTargetCollision(collision);
        }
    }

    public virtual void hitTargetCollision(Collider2D collision)
    {
        this.skill.hitIt(collision, this.percentage);
    }

    public void hitTarget()
    {
        if (this.skill.target != null) this.skill.hitIt(this.skill.target);
    }

    public void hitAllCharacters()
    {
        List<Character> targets = CollisionUtil.getAffectedCharacters(this.skill);
        foreach (Character target in targets)
        {
            if (percentage > 0) this.skill.hitIt(target, percentage);
        }
    }
}
