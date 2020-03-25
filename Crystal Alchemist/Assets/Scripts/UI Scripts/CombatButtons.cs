using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

[System.Serializable]
public class ButtonConfigUI
{
    public enumButton buttonType;
    public Image skillIconButton;
    public Image iconButton;
    public TextMeshProUGUI cooldown;
    public Image iconButtonTrans;
    public TextMeshProUGUI ammo;
}

public class CombatButtons : MonoBehaviour
{
    private Player player;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private List<ButtonConfigUI> buttonUIs = new List<ButtonConfigUI>();


    private void Awake()
    {
        this.player = this.playerStats.player;
    }

    void Start()
    {
        setButtonSkillImages();
    }

    public void setButtonSkillImages()
    {
        foreach(ButtonConfigUI buttonConfigUI in this.buttonUIs)
        {
            setButton(buttonConfigUI.buttonType, buttonConfigUI.skillIconButton, buttonConfigUI.iconButton);
        }
    }

    private void FixedUpdate()
    {
        foreach (ButtonConfigUI buttonConfigUI in this.buttonUIs)
        {
            setButton(buttonConfigUI.buttonType, buttonConfigUI.skillIconButton, buttonConfigUI.iconButton);
            updateButton(buttonConfigUI);
        }
    }

    private void setFontSize(TextMeshProUGUI textfield)
    {
        if (textfield.text == "X") textfield.fontSize = 75;
        else if (textfield.text == "[+]") textfield.fontSize = 50;
        else textfield.fontSize = 48;
    }
    
    private void updateButton(ButtonConfigUI configUI)
    {
        Ability ability = this.player.GetComponent<PlayerAbilities>().buttons.GetAbilityFromButton(configUI.buttonType);

        if (ability != null)
        {
            configUI.iconButton.enabled = true;
            float cooldownLeft = ability.cooldownLeft / (this.player.timeDistortion * this.player.spellspeed);
            float cooldownValue = ability.cooldown / (this.player.timeDistortion * this.player.spellspeed);

            SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();

            if (senderModule != null && senderModule.costs.item != null)
                configUI.ammo.text = (int)this.player.getResource(senderModule.costs) + "";
            else configUI.ammo.text = "";

            if (!ability.enabled || !ability.canUseAbility(this.player))
            {
                //ist Skill nicht einsetzbar (kein Mana oder bereits aktiv)
                string cooldownText = "X";
                configUI.cooldown.text = cooldownText;
                setFontSize(configUI.cooldown);

                configUI.cooldown.color = new Color(0.8f, 0, 0);
                configUI.cooldown.outlineColor = new Color32(75, 0, 0, 255);
                configUI.cooldown.outlineWidth = 0.25f;
                configUI.skillIconButton.color = new Color(1f, 1f, 1f, 0.2f);
                configUI.iconButton.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else if (ability.targetingSystem != null && ability.state == AbilityState.lockOn)
            {
                //ist Skill in Zielerfassung
                string cooldownText = "[+]";
                configUI.cooldown.text = cooldownText;
                setFontSize(configUI.cooldown);

                configUI.cooldown.color = new Color(0.8f, 0.75f, 0);
                configUI.cooldown.outlineColor = new Color32(75, 65, 0, 255);
                configUI.cooldown.outlineWidth = 0.25f;
                configUI.skillIconButton.color = new Color(1f, 1f, 1f, 0.2f);
                configUI.iconButton.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else if (cooldownLeft > 0 && cooldownValue > 0.5f)
            {
                //Ist Skill in der Abklingzeit
                string cooldownText = FormatUtil.setDurationToString(cooldownLeft);
                configUI.cooldown.text = cooldownText;
                setFontSize(configUI.cooldown);

                configUI.iconButton.fillAmount = 1 - (cooldownLeft / cooldownValue);
                configUI.cooldown.color = new Color(1f, 1f, 1f, 1f);
                configUI.cooldown.outlineColor = new Color32(75, 75, 75, 255);
                configUI.cooldown.outlineWidth = 0.25f;
                configUI.skillIconButton.color = new Color(1f, 1f, 1f, 0.2f);
                configUI.iconButton.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                //Skill ist einsatzbereit
                configUI.cooldown.text = "";
                setFontSize(configUI.cooldown);

                configUI.cooldown.outlineColor = new Color32(75, 75, 75, 255);
                configUI.skillIconButton.color = new Color(1f, 1f, 1f, 1f);
                configUI.iconButton.color = new Color(1f, 1f, 1f, 1f);
                configUI.iconButton.fillAmount = 1;
            }
        }
        else
        {
            configUI.iconButton.enabled = false;
            configUI.iconButtonTrans.enabled = true;
            configUI.ammo.text = "";
        }
    }

    private void setButton(enumButton buttonInput, Image skillUI, Image buttonUI)
    {
        Ability ability = this.player.GetComponent<PlayerAbilities>().buttons.GetAbilityFromButton(buttonInput);

        if (ability == null || ability.skill == null)
        {
            skillUI.gameObject.SetActive(false);
            buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
        }
        else
        {
            skillUI.gameObject.SetActive(true);
            if (ability.info != null) skillUI.sprite = ability.info.icon;
            buttonUI.color = new Color(1f, 1f, 1f, 1f);
            buttonUI.fillAmount = 1;
        }
    }
}
