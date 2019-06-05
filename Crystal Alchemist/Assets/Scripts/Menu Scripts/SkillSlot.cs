using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField]
    private Image image;

    public StandardSkill skill;

    public void setSkill(StandardSkill skill)
    {
        this.skill = skill;
        this.image.sprite = this.skill.icon;
    }

}
