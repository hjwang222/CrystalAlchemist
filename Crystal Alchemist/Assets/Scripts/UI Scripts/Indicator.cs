using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Indicator : MonoBehaviour
{
    [HideInInspector]
    public StandardSkill skill;

    [Required]
    public SpriteRenderer indicatorRenderer;

    [Required]
    public Animator animator;

    public virtual void Start()
    {

    }

    public void setSkill(StandardSkill skill)
    {
        this.skill = skill;
        if (skill != null)
        {
            if (skill.useCustomColor) this.indicatorRenderer.color = skill.indicatorColor;
        }
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
