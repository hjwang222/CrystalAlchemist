using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillSlot : MonoBehaviour
{    
    public Image image;
    public Skill skill;
    public int ID;

    private void Awake()
    {
        // this.ID = ((int.Parse(this.gameObject.transform.parent.name.Replace("Page ",""))-1)*10)+ int.Parse(this.gameObject.transform.name);
        this.ID = (this.gameObject.transform.parent.transform.parent.transform.GetSiblingIndex() * 10) + (this.gameObject.transform.GetSiblingIndex() + 1);
    }

    public void setSkill(Skill skill)
    {
        if (skill == null || skill.GetComponent<SkillBookModule>() == null)
            //this.image.color = new Color(1f, 1f, 1f, 0.2f);
            this.image.enabled = false;
        else
        {
            this.skill = skill;
            this.image.enabled = true;
            this.image.sprite = skill.GetComponent<SkillBookModule>().icon;
            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }    
}
