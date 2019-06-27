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

    [BoxGroup("Buttons")]
    [SerializeField]
    private GameObject previousPage;
    [BoxGroup("Buttons")]
    [SerializeField]
    private TextMeshProUGUI pageText;
    [BoxGroup("Buttons")]
    [SerializeField]
    private GameObject nextPage;

    private float delay = 0.3f;

    [HideInInspector]
    public StandardSkill selectedSkill;

    #endregion


    #region Unity Functions

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        setSkillsToSlots(SkillType.physical);
        setSkillsToSlots(SkillType.magical);
        setSkillsToSlots(SkillType.item);
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
        if (slot.skill != null)
        {
            this.skillDetailsName.text = "Name: " + slot.skill.skillName;

            float strength = 0;
            if (slot.skill.affectedResources.Count > 0) strength = Mathf.Abs(slot.skill.affectedResources[0].amount);
            this.skillDetailsStrength.text = "Stärke: " + strength + "";

            this.skillDetailsCost.text = "Kosten: " + Mathf.Abs(slot.skill.addResourceSender) + "";

            if (slot.skill.statusEffects.Count > 0)
            {
                this.StatusEffects.enabled = true;
                this.StatusEffects.sprite = slot.skill.statusEffects[0].iconSprite;
            }
            else this.StatusEffects.enabled = false;
        }
        else
        {
            hideSkillDetails();
        }
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

        setPage(0);
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

    public void setPage(int value)
    {
        GameObject activeCategory = null;

        if (this.physicalSkills.activeInHierarchy) activeCategory = this.physicalSkills;
        else if (this.magicalSkills.activeInHierarchy) activeCategory = this.magicalSkills;
        else if (this.itemSkills.activeInHierarchy) activeCategory = this.itemSkills;

        if (activeCategory != null)
        {
            int activeIndex = 0;
            int pagesCount = activeCategory.transform.childCount-1;

            for (int i = 0; i <= pagesCount; i++)
            {
                GameObject page = activeCategory.transform.GetChild(i).gameObject;
                if (page.activeInHierarchy) activeIndex = page.transform.GetSiblingIndex();
            }

            if (activeIndex + value < activeCategory.transform.childCount && activeIndex + value >= 0)
            {
                activeIndex += value;
            
                for (int i = 0; i < activeCategory.transform.childCount; i++)
                {
                    GameObject page = activeCategory.transform.GetChild(i).gameObject;
                    page.SetActive(false);
                    if (activeIndex == page.transform.GetSiblingIndex()) page.SetActive(true);
                }

                this.nextPage.SetActive(true);
                this.previousPage.SetActive(true);

                if (activeIndex == 0)
                {
                    this.previousPage.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.nextPage);
                }
                if (activeIndex == pagesCount)
                {
                    this.nextPage.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(this.previousPage);
                }

                if(!this.nextPage.activeInHierarchy && !this.previousPage.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);                    
                }

                this.pageText.text = (activeIndex + 1) + "/" + (pagesCount + 1);
            } 
        }
    }

    #endregion


    private void setSkillsToSlots(SkillType category)
    {
        GameObject categoryGameobject = this.itemSkills;
        if (category == SkillType.physical) categoryGameobject = this.physicalSkills;
        else if (category == SkillType.magical) categoryGameobject = this.magicalSkills;

        for(int i = 0; i < categoryGameobject.transform.childCount; i++)
        {
            GameObject page = categoryGameobject.transform.GetChild(i).gameObject;

            if (page.activeInHierarchy)
            {
                for (int ID = 0; ID < page.transform.childCount; ID++)
                {
                    GameObject slot = page.transform.GetChild(ID).gameObject;
                    StandardSkill skill = Utilities.getSkillByID(this.player.skillSet, slot.GetComponent<SkillSlot>().ID, category);                    
                    slot.GetComponent<SkillSlot>().setSkill(skill);
                }

                if (page.transform.GetSiblingIndex() > 0) page.SetActive(false);
            }
        }
    }

    /*
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
    }*/
}
