using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillSlot : MonoBehaviour
{    
    public Image image;
    public Ability ability;
    public int ID;

    private void Awake()
    {
        this.ID = (this.gameObject.transform.parent.transform.parent.transform.GetSiblingIndex() * 10) + (this.gameObject.transform.GetSiblingIndex() + 1);
    }

    public void setSkill(Ability ability)
    {
        if (ability == null || ability.info == null) this.image.enabled = false;
        else
        {
            this.ability = ability;
            this.image.enabled = true;
            this.image.sprite = ability.info.icon;
            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }    
}
