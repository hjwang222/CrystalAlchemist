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
    private enumButton button;
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

        if (skill != null)
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
        skillMenu.selectSkillFromSkillSet(null);
        setImage();
        this.newAssignedSkillSignal.Raise();
    }

    private StandardSkill getSkill()
    {
        switch (this.button)
        {
            case enumButton.AButton: return this.player.AButton;
            case enumButton.BButton: return this.player.BButton;
            case enumButton.XButton: return this.player.XButton;
            case enumButton.YButton: return this.player.YButton;
            default: return this.player.RBButton;
        }
    }

    private void setSkill(StandardSkill skill)
    {
        switch (this.button)
        {
            case enumButton.AButton: this.player.AButton = skill; break;
            case enumButton.BButton: this.player.BButton = skill; break;
            case enumButton.XButton: this.player.XButton = skill; break;
            case enumButton.YButton: this.player.YButton = skill; break;
            default: this.player.RBButton = skill; break;
        }
    }
}
