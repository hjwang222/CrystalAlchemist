using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuActiveSlots : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private Image AButtonSkill;
    [SerializeField]
    private Image BButtonSkill;
    [SerializeField]
    private Image XButtonSkill;
    [SerializeField]
    private Image YButtonSkill;
    [SerializeField]
    private Image RBButtonSkill;

    // Start is called before the first frame update
    void Start()
    {
        
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();

        updateSkillImage();
    }

    public void updateSkillImage()
    {
        setImage(AButtonSkill, this.player.AButton);
        setImage(BButtonSkill, this.player.BButton);
        setImage(XButtonSkill, this.player.XButton);
        setImage(YButtonSkill, this.player.YButton);
        setImage(RBButtonSkill, this.player.RBButton);
    }

    private void setImage(Image image, StandardSkill skill)
    {
        if(skill != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = skill.icon;
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }
}
