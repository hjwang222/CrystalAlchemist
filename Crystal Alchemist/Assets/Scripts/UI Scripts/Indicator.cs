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

    /*
    private void Update()
    {
        if(this.skill != null) this.transform.position = this.skill.transform.position;
    }
    */

    public void setSkill(StandardSkill skill)
    {
        this.skill = skill;
        if (skill != null)
        {
            if (skill.useCustomColor) this.indicatorRenderer.color = skill.indicatorColor;
            this.transform.position = this.skill.transform.position;
        }
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
