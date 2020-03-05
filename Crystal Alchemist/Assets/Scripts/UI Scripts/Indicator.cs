using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.LWRP;

public class Indicator : MonoBehaviour
{
    [HideInInspector]
    public Skill skill;

    [Required]
    public SpriteRenderer indicatorRenderer;

    public UnityEngine.Experimental.Rendering.Universal.Light2D light;

    public virtual void Start()
    {
        
    }

    public void setSkill(Skill skill)
    {
        this.skill = skill;
        if (skill != null && skill.GetComponent<SkillIndicatorModule>() != null)
        {
            if (skill.GetComponent<SkillIndicatorModule>().useCustomColor)
            {
                this.indicatorRenderer.color = skill.GetComponent<SkillIndicatorModule>().indicatorColor;
                if(this.light != null) this.light.color = skill.GetComponent<SkillIndicatorModule>().indicatorColor;
            }
            this.transform.position = this.skill.transform.position;
        }
    }

    public virtual void Update()
    {
        if(this.skill == null || this.skill != null && this.skill.sender.currentState == CharacterState.dead)
        {
            DestroyIt();
        }
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
