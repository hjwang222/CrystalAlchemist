using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingAnimation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Animator Value = 'Time'")]
    private bool matchAnimationProgress;

    [SerializeField]
    private Animator animator;

    private StandardSkill skill;
    private Character character;
    private float progress;

    public void setCastingAnimation(StandardSkill skill, Character character)
    {
        this.skill = skill;
        this.character = character;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = this.character.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        this.progress = this.skill.holdTimer * 100 / this.skill.cast;

        if (this.matchAnimationProgress)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Time", this.progress);
        }
    }
}
