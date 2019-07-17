using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class CombatButtons : MonoBehaviour
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

    [FoldoutGroup("RB Button UI", expanded: false)]
    [SerializeField]
    private Image iconRBButton;
    [FoldoutGroup("RB Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconRBButton;
    [FoldoutGroup("RB Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI cooldownRB;
    [FoldoutGroup("RB Button UI", expanded: false)]
    [SerializeField]
    private TextMeshProUGUI ammoRB;
    [FoldoutGroup("RB Button UI", expanded: false)]
    [SerializeField]
    private Image skillIconRBButtonTrans;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        setButtonSkillImages();
    }

    private void OnEnable()
    {
        setButtonSkillImages();
    }

    public void setButtonSkillImages()
    {

        setButton(this.player.AButton, this.skillIconAButton, this.iconAButton);
        setButton(this.player.BButton, this.skillIconBButton, this.iconBButton);
        setButton(this.player.XButton, this.skillIconXButton, this.iconXButton);
        setButton(this.player.YButton, this.skillIconYButton, this.iconYButton);
        setButton(this.player.RBButton, this.skillIconRBButton, this.iconRBButton);
    }

    private void FixedUpdate()
    {
        updateButton(this.skillIconAButton, this.iconAButton, this.cooldownA, this.player.AButton, this.ammoA, this.skillIconAButtonTrans);
        updateButton(this.skillIconBButton, this.iconBButton, this.cooldownB, this.player.BButton, this.ammoB, this.skillIconBButtonTrans);
        updateButton(this.skillIconXButton, this.iconXButton, this.cooldownX, this.player.XButton, this.ammoX, this.skillIconXButtonTrans);
        updateButton(this.skillIconYButton, this.iconYButton, this.cooldownY, this.player.YButton, this.ammoY, this.skillIconYButtonTrans);
        updateButton(this.skillIconRBButton, this.iconRBButton, this.cooldownRB, this.player.RBButton, this.ammoRB, this.skillIconRBButtonTrans);
    }


    private void updateButton(Image skillUI, Image buttonUI, TextMeshProUGUI cooldown, StandardSkill skill, TextMeshProUGUI ammo, Image buttonUITrans)
    {
        if (skill != null)
        {       
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
                string cooldownText = Utilities.Format.setDurationToString(cooldownLeft) + "s";

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
