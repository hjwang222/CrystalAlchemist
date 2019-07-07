using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class addSkill : affectResourceStatusEffect
{
    [FoldoutGroup("Add Skill", expanded: false)]
    [SerializeField]
    private StandardSkill addSkillToTarget;

    public override void init()
    {
        base.init();
        StandardSkill skill = Utilities.instantiateSkill(this.addSkillToTarget, this.target);
        skill.duration = this.statusEffectDuration;
    }
}
