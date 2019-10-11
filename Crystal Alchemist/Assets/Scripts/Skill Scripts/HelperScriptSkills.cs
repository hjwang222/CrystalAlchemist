using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HelperScriptSkills : MonoBehaviour
{
    [SerializeField]
    [Required]
    private Skill skill;

    public void PlaySoundEffect(AudioClip clip)
    {
        this.skill.PlaySoundEffect(clip);
    }

    public void PlaySoundEffectOnce(AudioClip clip)
    {
        this.skill.PlaySoundEffectOnce(clip);
    }

    public void ActivateIt()
    {
        this.skill.ActivateIt();
    }

    public void SetTriggerActive(int value)
    {
        this.skill.SetTriggerActive(value);
    }

    public void DestroyIt()
    {
        this.skill.DestroyIt();
    }

    public void DestroyDelay(float delay)
    {
        this.skill.DestroyIt(delay);
    }

    public void showIndicator()
    {
        if(this.skill.GetComponent<SkillIndicatorModule>() != null) this.skill.GetComponent<SkillIndicatorModule>().showIndicator();
    }

    public void hideIndicator()
    {
        if (this.skill.GetComponent<SkillIndicatorModule>() != null) this.skill.GetComponent<SkillIndicatorModule>().hideIndicator();
    }

    public void showCastingAnimation()
    {
        if (this.skill.GetComponent<SkillAnimationModule>() != null) this.skill.GetComponent<SkillAnimationModule>().showCastingAnimation();
    }

    public void hideCastingAnimation()
    {
        if (this.skill.GetComponent<SkillAnimationModule>() != null) this.skill.GetComponent<SkillAnimationModule>().hideCastingAnimation();
    }

    public void ResetRotation()
    {
        this.skill.resetRotation();
    }
}
