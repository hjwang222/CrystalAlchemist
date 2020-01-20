using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class CombatButtons : MonoBehaviour
{
    private Player player;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

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
        this.player = this.playerStats.player;
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
        updateButton(this.skillIconAButton, this.iconAButton, this.cooldownA, this.player.AButton, this.ammoA, this.skillIconAButtonTrans, this.player.GetComponent<PlayerAttacks>().isButtonUsable("A-Button"));
        updateButton(this.skillIconBButton, this.iconBButton, this.cooldownB, this.player.BButton, this.ammoB, this.skillIconBButtonTrans, this.player.GetComponent<PlayerAttacks>().isButtonUsable("B-Button"));
        updateButton(this.skillIconXButton, this.iconXButton, this.cooldownX, this.player.XButton, this.ammoX, this.skillIconXButtonTrans, this.player.GetComponent<PlayerAttacks>().isButtonUsable("X-Button"));
        updateButton(this.skillIconYButton, this.iconYButton, this.cooldownY, this.player.YButton, this.ammoY, this.skillIconYButtonTrans, this.player.GetComponent<PlayerAttacks>().isButtonUsable("Y-Button"));
        updateButton(this.skillIconRBButton, this.iconRBButton, this.cooldownRB, this.player.RBButton, this.ammoRB, this.skillIconRBButtonTrans, this.player.GetComponent<PlayerAttacks>().isButtonUsable("RB-Button"));
    }

    private void setFontSize(TextMeshProUGUI textfield)
    {
        if (textfield.text == "X") textfield.fontSize = 75;
        else if (textfield.text == "[+]") textfield.fontSize = 50;
        else textfield.fontSize = 48;
    }


    private void updateButton(Image skillUI, Image buttonUI, TextMeshProUGUI cooldown, Skill skill, TextMeshProUGUI ammo, Image buttonUITrans, bool isUsable)
    {
        if (skill != null)
        {
            buttonUI.enabled = true;
            float cooldownLeft = skill.cooldownTimeLeft / (player.timeDistortion * player.spellspeed);
            float cooldownValue = skill.cooldown / (player.timeDistortion * player.spellspeed);

            SkillSenderModule senderModule = skill.GetComponent<SkillSenderModule>();

            if (senderModule != null && senderModule.item != null) ammo.text = (int)player.getResource(senderModule.resourceType, senderModule.item)+"";
            else ammo.text = "";            

            if (!isUsable ||
                this.player.currentState == CharacterState.attack ||
                this.player.currentState == CharacterState.defend ||
                this.player.currentState == CharacterState.knockedback ||
                this.player.currentState == CharacterState.inDialog ||
                this.player.currentState == CharacterState.respawning ||
                (!skill.isResourceEnough(player))
                || CustomUtilities.Skills.getAmountOfSameSkills(skill, player.activeSkills, player.activePets) >= skill.maxAmounts
                || cooldownLeft == CustomUtilities.maxFloatInfinite
                || !skill.basicRequirementsExists)
            {
                //ist Skill nicht einsetzbar (kein Mana oder bereits aktiv)
                string cooldownText = "X";
                cooldown.text = cooldownText;
                setFontSize(cooldown);

                cooldown.color = new Color(0.8f, 0, 0);
                cooldown.outlineColor = new Color32(75, 0, 0, 255);
                cooldown.outlineWidth = 0.25f;
                skillUI.color = new Color(1f, 1f, 1f, 0.2f);
                buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else if (player.activeLockOnTarget != null
              && player.activeLockOnTarget.getSkill() == skill)
            {
                //ist Skill in Zielerfassung
                string cooldownText = "[+]";
                cooldown.text = cooldownText;
                setFontSize(cooldown);

                cooldown.color = new Color(0.8f, 0.75f, 0);
                cooldown.outlineColor = new Color32(75, 65, 0, 255);
                cooldown.outlineWidth = 0.25f;
                skillUI.color = new Color(1f, 1f, 1f, 0.2f);
                buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else if (cooldownLeft > 0 && cooldownValue > 0.5f)
            {
                //Ist Skill in der Abklingzeit
                string cooldownText = CustomUtilities.Format.setDurationToString(cooldownLeft);
                cooldown.text = cooldownText;
                setFontSize(cooldown);

                buttonUI.fillAmount = 1 - (cooldownLeft / cooldownValue);
                cooldown.color = new Color(1f, 1f, 1f, 1f);
                cooldown.outlineColor = new Color32(75, 75, 75, 255);
                cooldown.outlineWidth = 0.25f;
                skillUI.color = new Color(1f, 1f, 1f, 0.2f);
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                //Skill ist einsatzbereit
                cooldown.text = "";
                setFontSize(cooldown);

                cooldown.outlineColor = new Color32(75, 75, 75, 255);
                skillUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.color = new Color(1f, 1f, 1f, 1f);
                buttonUI.fillAmount = 1;                
            }
        }
        else
        {
            buttonUI.enabled = false;
            buttonUITrans.enabled = true;
            ammo.text = "";
        }
    }

    private void setButton(Skill skill, Image skillUI, Image buttonUI)
    {
        if (skill == null)
        {
            skillUI.gameObject.SetActive(false);
            buttonUI.color = new Color(1f, 1f, 1f, 0.2f);
        }
        else
        {
            skillUI.gameObject.SetActive(true);
            if(skill.GetComponent<SkillBookModule>() != null) skillUI.sprite = skill.GetComponent<SkillBookModule>().icon;
            buttonUI.color = new Color(1f, 1f, 1f, 1f);
            buttonUI.fillAmount = 1;
        }
    }

}
