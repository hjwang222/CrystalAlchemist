using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillAnimationModule : SkillModule
{
    public string animationTriggerName = "";

    [Space(10)]
    public CastingAnimation castingAnimation;

    [ShowIf("castingAnimation", null)]
    public string castingAnimationCharacterKey;

    private CastingAnimation activeCastingAnimation;

    private void Start()
    {
        this.skill.sender.startAttackAnimation(this.animationTriggerName);
    }

    public void showCastingAnimation()
    {
        if (this.castingAnimation != null
        && this.activeCastingAnimation == null
        && this.skill.sender != null)
        {
            this.activeCastingAnimation = Instantiate(this.castingAnimation, this.skill.sender.transform);
            this.activeCastingAnimation.setCastingAnimation(this.skill, this.skill.sender);
            Utilities.UnityUtils.SetAnimatorParameter(this.skill.sender.animator, this.castingAnimationCharacterKey, true);
        }
    }

    public void hideCastingAnimation()
    {
        if (this.activeCastingAnimation != null)
        {
            Destroy(this.activeCastingAnimation.gameObject);
            this.activeCastingAnimation = null;
            Utilities.UnityUtils.SetAnimatorParameter(this.skill.sender.animator, this.castingAnimationCharacterKey, false);
        }
    }
}
