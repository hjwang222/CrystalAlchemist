using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buttons : MonoBehaviour
{
    public ButtonConfig buttonConfig;
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
        setButton(this.buttonConfig.buttonA.icon, this.buttonConfig.buttonAIcon, this.skillIconAButton, this.iconAButton);
        setButton(this.buttonConfig.buttonB.icon, this.buttonConfig.buttonBIcon, this.skillIconBButton, this.iconBButton);
        setButton(this.buttonConfig.buttonX.icon, this.buttonConfig.buttonXIcon, this.skillIconXButton, this.iconXButton);
        setButton(this.buttonConfig.buttonY.icon, this.buttonConfig.buttonYIcon, this.skillIconYButton, this.iconYButton);
    }

    private void Update()
    {
        try
        {
            updateButton(this.skillIconAButton, this.iconAButton, this.cooldownA, this.buttonConfig.buttonA);
            updateButton(this.skillIconBButton, this.iconBButton, this.cooldownB, this.buttonConfig.buttonB);
            updateButton(this.skillIconXButton, this.iconXButton, this.cooldownX, this.buttonConfig.buttonX);
            updateButton(this.skillIconYButton, this.iconYButton, this.cooldownY, this.buttonConfig.buttonY);
        }
        catch { }
    }

    private void updateButton(Image skillUI, Image buttonUI, TextMeshProUGUI text, StandardSkill skill)
    {
        float cooldownLeft = skill.cooldownTimeLeft / (player.timeDistortion * player.spellspeed);
        float cooldown = skill.cooldown / (player.timeDistortion * player.spellspeed);

        if ((player.mana + skill.addManaSender < 0 && skill.addManaSender != -Utilities.maxFloatInfinite) 
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

    private void setButton(Sprite skillIcon, Sprite buttonIcon, Image skillUI, Image buttonUI)
    {
        if(skillIcon == null)
        {
            skillUI.enabled = false;
        }
        else
        {
            skillUI.sprite = skillIcon;
        }

        if (buttonIcon == null)
        {
            buttonUI.enabled = false;
        }
        else
        {
            buttonUI.sprite = buttonIcon;
        }
    }

}
