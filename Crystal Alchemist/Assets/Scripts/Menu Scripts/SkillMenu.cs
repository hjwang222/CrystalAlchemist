using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using System.Linq;

public class SkillMenu : MonoBehaviour
{
    #region Attributes
    private Player player;

    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private GameObject dummySlot;
    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject cursor;
    [BoxGroup("Mandatory")]
    [SerializeField]
    private TextMeshProUGUI categoryText;

    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject physicalSkills;
    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject magicalSkills;
    [BoxGroup("Tabs")]
    [SerializeField]
    private GameObject itemSkills;

    [BoxGroup("Detail-Ansicht")]
    [SerializeField]
    private TextMeshProUGUI skillDetailsName;
    [BoxGroup("Detail-Ansicht")]
    [SerializeField]
    private TextMeshProUGUI skillDetailsStrength;
    [BoxGroup("Detail-Ansicht")]
    [SerializeField]
    private TextMeshProUGUI skillDetailsCost;
    [BoxGroup("Detail-Ansicht")]
    [SerializeField]
    private Image StatusEffects;

    [BoxGroup("Skill Slots")]
    [SerializeField]
    private GameObject slotA;
    [BoxGroup("Skill Slots")]
    [SerializeField]
    private GameObject slotB;
    [BoxGroup("Skill Slots")]
    [SerializeField]
    private GameObject slotX;
    [BoxGroup("Skill Slots")]
    [SerializeField]
    private GameObject slotY;
    [BoxGroup("Skill Slots")]
    [SerializeField]
    private GameObject slotRB;

    private float delay = 0.3f;

    [HideInInspector]
    public StandardSkill selectedSkill;

    #endregion


    #region Unity Functions

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        setSlots();
        showCategory(1);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Inventory")) exitMenu();
    }

    private void OnEnable()
    {
        this.cursor.SetActive(true);
        this.player.currentState = CharacterState.inDialog;        
    }

    private void OnDisable()
    {
        this.cursor.SetActive(false);
    }

    #endregion


    #region OnClickTrigger
    public void showSkillDetails(SkillSlot slot)
    {        
            this.skillDetailsName.text = "Name: " + slot.skill.skillName;
            this.skillDetailsStrength.text = "Stärke: " + Mathf.Abs(slot.skill.addLifeTarget) + "";
            this.skillDetailsCost.text = "Kosten: " + Mathf.Abs(slot.skill.addResourceSender) + "";

        if (slot.skill.statusEffects.Count > 0)
        {
            this.StatusEffects.enabled = true;
            this.StatusEffects.sprite = slot.skill.statusEffects[0].iconSprite;
        }
        else this.StatusEffects.enabled = false;
    }

    public void hideSkillDetails()
    {
        this.skillDetailsName.text = "Name: ";
        this.skillDetailsStrength.text = "Stärke: ";
        this.skillDetailsCost.text = "Kosten: ";

        this.StatusEffects.enabled = false;
    }

    public void showCategory(int category)
    {        
        this.physicalSkills.SetActive(false);
        this.magicalSkills.SetActive(false);
        this.itemSkills.SetActive(false);
        
        switch (category)
        {
            case 1: this.physicalSkills.SetActive(true); this.categoryText.text = "Waffen"; break;
            case 2: this.magicalSkills.SetActive(true); this.categoryText.text = "Zauber"; break;
            default: this.itemSkills.SetActive(true); this.categoryText.text = "Items"; break;
        }
    }

    public void selectSkillFromSkillSet(SkillSlot skillSlot)
    {
        this.selectedSkill = skillSlot.skill;
    }

    public void exitMenu()
    {
        this.player.delay(this.delay);
        this.transform.parent.gameObject.SetActive(false);
    }



    #endregion


    private void setSlots()
    {
        this.dummySlot.SetActive(true);

        List<StandardSkill> sortedByIndex 
            = this.player.skillSet.ToArray().OrderBy(o => (((int)(o.category)*1000)+o.orderIndex)).ToList<StandardSkill>();

        for (int i = 0; i < sortedByIndex.Count; i++)
        {
            StandardSkill skill = sortedByIndex[i];
            GameObject categoryGameobject = this.itemSkills;

            if (skill.category == SkillType.physical) categoryGameobject = this.physicalSkills;
            else if (skill.category == SkillType.magical) categoryGameobject = this.magicalSkills;

            GameObject temp = Instantiate(this.dummySlot, categoryGameobject.transform);
            temp.GetComponent<SkillSlot>().setSkill(skill);
            temp.name = skill.skillName + "-Slot";
        }

        this.dummySlot.SetActive(false);
    }
}
