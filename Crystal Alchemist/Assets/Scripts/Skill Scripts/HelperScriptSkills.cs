using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperScriptSkills : MonoBehaviour
{
    [SerializeField]
    private StandardSkill skill;

    public void PlayStartSoundEffect()
    {
        this.skill.PlayStartSoundEffect();
    }

    public void PlayAnimatorSoundEffect()
    {
        this.skill.PlayAnimatorSoundEffect();
    }

    public void PlayEndSoundEffect()
    {
        this.skill.PlayEndSoundEffect();
    }

    public void ActivateIt()
    {
        this.skill.ActivateIt();
    }

    public void SetTriggerActive(int value)
    {
        this.skill.SetTriggerActive(value);
    }

    public virtual void DestroyIt()
    {
        this.skill.DestroyIt();
    }
}
