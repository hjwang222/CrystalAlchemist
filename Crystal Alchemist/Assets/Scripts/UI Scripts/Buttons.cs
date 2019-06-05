using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buttons : MonoBehaviour
{
    private Player player;

    [Header("A Button UI")]
    public Image iconAButton;
    public Image skillIconAButton;
    public TextMeshProUGUI cooldownA;

    [Header("B Button UI")]
    public Image iconBButton;
    public Image skillIconBButton;
    public TextMeshProUGUI cooldownB;

    [Header("X Button UI")]
    public Image iconXButton;
    public Image skillIconXButton;
    public TextMeshProUGUI cooldownX;

    [Header("Y Button UI")]
    public Image iconYButton;
    public Image skillIconYButton;
    public TextMeshProUGUI cooldownY;

    // Start is called before the first frame update

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        setButtonSkillImages();
    }

    public void setButtonSkillImages()
    {
        setButton(this.player.AButton, this.skillIconAButton, this.iconAButton);
        setButton(this.player.BButton, this.skillIconBButton, this.iconBButton);
        setButton(this.player.XButton, this.skillIconXButton, this.iconXButton);
        setButton(this.player.YButton, this.skillIconYButton, this.iconYButton);
    }

    private void Update()
    {
        try
        {
            updateButton(this.skillIconAButton, this.iconAButton, this.cooldownA, this.player.AButton);
            updateButton(this.skillIconBButton, this.iconBButton, this.cooldownB, this.player.BButton);
            updateButton(this.skillIconXButton, this.iconXButton, this.cooldownX, this.player.XButton);
            updateButton(this.skillIconYButton, this.iconYButton, this.cooldownY, this.player.YButton);
        }
        catch { }
    }

    private void updateButton(Image skillUI, Image buttonUI, TextMeshProUGUI text, StandardSkill skill)
    {
        float cooldownLeft = skill.cooldownTimeLeft / (player.timeDistortion * player.spellspeed);
        float cooldown = skill.cooldown / (player.timeDistortion * player.spellspeed);

        if ((player.getResource(skill.resourceType, skill.item) + skill.addResourceSender < 0 && skill.addResourceSender != -Utilities.maxFloatInfinite) 
            || player.getAmountOfSameSkills(skill) >= skill.maxAmounts
            || cooldownLeft == Utilities.maxFloatInfinite)
        {
            //ist Skill nicht einsetzbar (kein Mana oder bereits aktiv)
            string cooldownText = "X";

            text.text = cooldownText;
            text.fontSize = 74;
            text.color = new Color(0.8f, 0, 0);
            text.outlineColor = new Color32(75, 0, 0, 255);
            text.outlineWidth = 0.25f;
            skillUI.color = new Color(1f, 1f, 1f, 0.2f);
            buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
        }
        else if (player.activeLockOnTarget != null
          && player.activeLockOnTarget.GetComponent<TargetingSystem>().skill == skill)
        {
            //ist Skill in Zielerfassung
            string cooldownText = "[+]";

            text.fontSize = 50;
            text.color = new Color(0.8f, 0.75f, 0);
            text.outlineColor = new Color32(75, 65, 0, 255);
            text.outlineWidth = 0.25f;
            text.text = cooldownText;
            skillUI.color = new Color(1f, 1f, 1f, 0.2f);
            buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
        }
        else if(cooldownLeft > 0 && cooldown > 0.5f)
        {
            //Ist Skill in der Abklingzeit
            string cooldownText = Utilities.setDurationToString(cooldownLeft) + "s";

            buttonUI.fillAmount = 1-(cooldownLeft / cooldown);

            text.fontSize = 50;
            text.color = new Color(1f,1f,1f,1f);
            text.outlineColor = new Color32(75, 75, 75, 255);
            text.outlineWidth = 0.25f;
            text.text = cooldownText;
            skillUI.color = new Color(1f, 1f, 1f, 0.2f);
            buttonUI.color = new Color(1f, 1f, 1f, 1f);
        }        
        else
        {
            //Skill ist einsatzbereit
            text.text = "";
            text.fontSize = 50;
            text.outlineColor = new Color32(75, 75, 75, 255);
            skillUI.color = new Color(1f, 1f, 1f, 1f);
            buttonUI.color = new Color(1f, 1f, 1f, 1f);
            buttonUI.fillAmount = 1;
        }
    }

    private void setButton(StandardSkill skill, Image skillUI, Image buttonUI)
    {
        if(skill == null)
        {
            skillUI.gameObject.SetActive(false);
        }
        else
        {
            skillUI.gameObject.SetActive(true);
            skillUI.sprite = skill.icon;
        }
    }

}
