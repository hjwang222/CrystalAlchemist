using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillSlot : MonoBehaviour
{
    [SerializeField]
    private Image image;
    public StandardSkill skill;
    public int ID;

    private void Awake()
    {
        this.ID = ((int.Parse(this.gameObject.transform.parent.name.Replace("Page ",""))-1)*10)+ int.Parse(this.gameObject.transform.name);
    }

    public void setSkill(StandardSkill skill)
    {
        if (skill == null)
            //this.image.color = new Color(1f, 1f, 1f, 0.2f);
            this.image.enabled = false;
        else
        {
            this.skill = skill;
            this.image.enabled = true;
            this.image.sprite = this.skill.icon;
            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }    
}
