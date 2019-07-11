using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class Buttons : MonoBehaviour
{
    private Player player;

    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private Image iconAButton;
    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconAButton;
    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI cooldownA;
    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI ammoA;
    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconAButtonTrans;
    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI action;
    [FoldoutGroup("A Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI confirm;

    [FoldoutGroup("B Button UI", expanded: false)]
    [SerializeField]
    private Image iconBButton;
    [FoldoutGroup("B Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconBButton;
    [FoldoutGroup("B Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI cooldownB;
    [FoldoutGroup("B Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI ammoB;
    [FoldoutGroup("B Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconBButtonTrans;
    [FoldoutGroup("B Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI cancel;

    [FoldoutGroup("X Button UI", expanded: false)]
    [SerializeField]
    private Image iconXButton;
    [FoldoutGroup("X Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconXButton;
    [FoldoutGroup("X Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI cooldownX;
    [FoldoutGroup("X Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI ammoX;
    [FoldoutGroup("X Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconXButtonTrans;

    [FoldoutGroup("Y Button UI", expanded: false)]
    [SerializeField]
    private Image iconYButton;
    [FoldoutGroup("Y Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconYButton;
    [FoldoutGroup("Y Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI cooldownY;
    [FoldoutGroup("Y Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI ammoY;
    [FoldoutGroup("Y Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconYButtonTrans;

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

    private void FixedUpdate()
    {
        updateButton(this.skillIconAButton, this.iconAButton, this.cooldownA, this.player.AButton, this.ammoA, this.action, this.skillIconAButtonTrans, this.confirm);
        updateButton(this.skillIconBButton, this.iconBButton, this.cooldownB, this.player.BButton, this.ammoB, null, this.skillIconBButtonTrans, this.cancel);
        updateButton(this.skillIconXButton, this.iconXButton, this.cooldownX, this.player.XButton, this.ammoX, null, this.skillIconXButtonTrans, null);
        updateButton(this.skillIconYButton, this.iconYButton, this.cooldownY, this.player.YButton, this.ammoY, null, this.skillIconYButtonTrans, null);
    }

    private void enableUI(Image skillUI, Image buttonUI, TextMeshProUGUI cooldown, TextMeshProUGUI ammo, Image buttonUITrans, bool value)
    {
        buttonUITrans.enabled = value;
        cooldown.enabled = value;
        skillUI.enabled = value;
        buttonUI.enabled = value;
        ammo.enabled = value;
    }

    private void updateButton(Image skillUI, Image buttonUI, TextMeshProUGUI cooldown, StandardSkill skill, TextMeshProUGUI ammo, TextMeshProUGUI action, Image buttonUITrans, TextMeshProUGUI menuAction)
    {
        if (action != null) action.enabled = false;
        if (menuAction != null) menuAction.enabled = false;

        enableUI(skillUI, buttonUI, cooldown, ammo, buttonUITrans, false);

        if (this.player.currentState == CharacterState.inMenu)
        {
            if (menuAction != null)
            {
                menuAction.enabled = true;
                buttonUI.enabled = true;
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.fillAmount = 1;
            }            
        }
        else if (this.player.currentState == CharacterState.inDialog)
        {
            
        }
        else if (this.player.currentState == CharacterState.interact)
        {
            if (action != null)
            {
                action.enabled = true;
                buttonUI.enabled = true;
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.fillAmount = 1;
            }                      
        }         
        else if (skill != null)
        {
            enableUI(skillUI, buttonUI, cooldown, ammo, buttonUITrans, true);

            float cooldownLeft = skill.cooldownTimeLeft / (player.timeDistortion * player.spellspeed);
            float cooldownValue = skill.cooldown / (player.timeDistortion * player.spellspeed);

            if (skill.item != null) ammo.text = (int)player.getResource(skill.resourceType, skill.item)+"";
            else ammo.text = "";            

            if (this.player.currentState == CharacterState.attack ||
                this.player.currentState == CharacterState.defend ||
                this.player.currentState == CharacterState.knockedback ||
                this.player.currentState == CharacterState.inDialog ||
                (player.getResource(skill.resourceType, skill.item) + skill.addResourceSender < 0 && skill.addResourceSender != -Utilities.maxFloatInfinite)
                || player.getAmountOfSameSkills(skill) >= skill.maxAmounts
                || cooldownLeft == Utilities.maxFloatInfinite)
            {
                //ist Skill nicht einsetzbar (kein Mana oder bereits aktiv)
                string cooldownText = "X";

                cooldown.text = cooldownText;
                cooldown.fontSize = 74;
                cooldown.color = new Color(0.8f, 0, 0);
                cooldown.outlineColor = new Color32(75, 0, 0, 255);
                cooldown.outlineWidth = 0.25f;
                skillUI.color = new Color(1f, 1f, 1f, 0.2f);
                buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else if (player.activeLockOnTarget != null
              && player.activeLockOnTarget.GetComponent<TargetingSystem>().skill == skill)
            {
                //ist Skill in Zielerfassung
                string cooldownText = "[+]";

                cooldown.fontSize = 50;
                cooldown.color = new Color(0.8f, 0.75f, 0);
                cooldown.outlineColor = new Color32(75, 65, 0, 255);
                cooldown.outlineWidth = 0.25f;
                cooldown.text = cooldownText;
                skillUI.color = new Color(1f, 1f, 1f, 0.2f);
                buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else if (cooldownLeft > 0 && cooldownValue > 0.5f)
            {
                //Ist Skill in der Abklingzeit
                string cooldownText = Utilities.setDurationToString(cooldownLeft) + "s";

                buttonUI.fillAmount = 1 - (cooldownLeft / cooldownValue);

                cooldown.fontSize = 50;
                cooldown.color = new Color(1f, 1f, 1f, 1f);
                cooldown.outlineColor = new Color32(75, 75, 75, 255);
                cooldown.outlineWidth = 0.25f;
                cooldown.text = cooldownText;
                skillUI.color = new Color(1f, 1f, 1f, 0.2f);
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                //Skill ist einsatzbereit
                cooldown.text = "";
                cooldown.fontSize = 50;
                cooldown.outlineColor = new Color32(75, 75, 75, 255);
                skillUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.fillAmount = 1;                
            }
        }
        else
        {
            buttonUITrans.enabled = true;
        }
    }

    private void setButton(StandardSkill skill, Image skillUI, Image buttonUI)
    {
        if (skill == null)
        {
            skillUI.gameObject.SetActive(false);
            buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
        }
        else
        {
            skillUI.gameObject.SetActive(true);
            skillUI.sprite = skill.icon;
            buttonUI.color = new Color(1f, 1f, 1f, 1f);
            buttonUI.fillAmount = 1;
        }
    }

}
