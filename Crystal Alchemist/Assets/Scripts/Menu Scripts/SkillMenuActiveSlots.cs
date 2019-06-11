using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuActiveSlots : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private Image skillImage;
    [SerializeField]
    private Button button;
    [SerializeField]
    private SimpleSignal newAssignedSkillSignal;

    void Start()
    {        
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        setImage();
    }

    private void setImage()
    {
        StandardSkill skill = getSkill();

        if(skill != null)
        {
            this.skillImage.gameObject.SetActive(true);
            this.skillImage.sprite = skill.icon;
        }
        else
        {
            this.skillImage.gameObject.SetActive(false);
        }
    }

    public void assignSkillToButton(SkillMenu skillMenu)
    {        
            setSkill(skillMenu.selectedSkill);
            skillMenu.selectedSkill = null;
            setImage();
            this.newAssignedSkillSignal.Raise();        
    }

    private StandardSkill getSkill()
    {
        switch (this.button)
        {
            case Button.AButton: return this.player.AButton;
            case Button.BButton: return this.player.BButton;
            case Button.XButton: return this.player.XButton;
            case Button.YButton: return this.player.YButton;
            default: return this.player.RBButton;
        }
    }

    private void setSkill(StandardSkill skill)
    {
        switch (this.button)
        {
            case Button.AButton: this.player.AButton = skill; break;
            case Button.BButton: this.player.BButton = skill; break;
            case Button.XButton: this.player.XButton = skill; break;
            case Button.YButton: this.player.YButton = skill; break;
            default: this.player.RBButton = skill; break;
        }
    }
}
