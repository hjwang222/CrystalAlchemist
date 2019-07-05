using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class addSkill : affectResource
{
    [FoldoutGroup("Add Skill", expanded: false)]
    [SerializeField]
    private StandardSkill addSkillToTarget;

    public override void init()
    {
        base.init();
        Utilities.instantiateSkill(this.addSkillToTarget, this.target);
    }
}
