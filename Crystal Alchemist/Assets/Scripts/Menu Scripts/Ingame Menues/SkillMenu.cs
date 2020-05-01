using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class SkillMenu : MenuControls
{
    #region Attributes

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private PlayerSkillset skillSet;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI categoryWeapons;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI categoryMagic;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI categoryItems;

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

    [BoxGroup("Buttons")]
    [SerializeField]
    private GameObject previousPage;
    [BoxGroup("Buttons")]
    [SerializeField]
    private TextMeshProUGUI pageText;
    [BoxGroup("Buttons")]
    [SerializeField]
    private GameObject nextPage;

    [HideInInspector]
    public Ability selectedAbility;

    #endregion


    #region Unity Functions

    public override void Start()
    {
        GameEvents.current.OnPage += SetPage;

        base.Start();

        setSkillsToSlots(SkillType.physical);
        setSkillsToSlots(SkillType.magical);
        setSkillsToSlots(SkillType.item);

        showCategory(1);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPage -= SetPage;
    }

    public override void OnCancel()
    {
        if (this.selectedAbility != null) selectSkillFromSkillSet(null);
        else if (this.cursor.infoBox.gameObject.activeInHierarchy) this.cursor.infoBox.Hide();
        else exitMenu();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        selectSkillFromSkillSet(null);
    }

    public override void OnDisable()
    {
        selectSkillFromSkillSet(null);
        base.OnDisable();
    }

    #endregion


    #region OnClickTrigger

    public void showSkillDetails(SkillSlot slot)
    {
        showSkillDetails(slot.ability);
    }

    public void showSkillDetails(SkillMenuActiveSlots slot)
    {
        showSkillDetails(slot.ability);
    }

    private void showSkillDetails(Ability ability)
    {
        if (ability != null)
        {
            SkillTargetModule targetModule = ability.skill.GetComponent<SkillTargetModule>();
            SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();

            this.skillDetailsName.text = ability.GetName();
            float strength = 0;

            if(senderModule != null)
            {
                this.skillDetailsCost.text = Mathf.Abs(senderModule.costs.amount).ToString("N1");
            }

            if (targetModule != null)
            {
                if (targetModule.affectedResources.Count > 0) strength = Mathf.Abs(targetModule.affectedResources[0].amount);
                this.skillDetailsStrength.text = strength + "";

                if (targetModule.statusEffects.Count > 0)
                {
                    this.StatusEffects.enabled = true;
                    this.StatusEffects.sprite = targetModule.statusEffects[0].iconSprite;
                }
                else this.StatusEffects.enabled = false;
            }
        }
        else
        {
            hideSkillDetails();
        }
    }

    public void hideSkillDetails()
    {        
        this.skillDetailsName.text = "";
        this.skillDetailsStrength.text = "";
        this.skillDetailsCost.text = "";

        this.StatusEffects.enabled = false;
    }

    public void showCategory(int category)
    {        
        this.physicalSkills.SetActive(false);
        this.categoryWeapons.gameObject.SetActive(false);
        this.magicalSkills.SetActive(false);
        this.categoryMagic.gameObject.SetActive(false);
        this.itemSkills.SetActive(false);
        this.categoryItems.gameObject.SetActive(false);

        switch (category)
        {
            case 1: this.physicalSkills.SetActive(true); this.categoryWeapons.gameObject.SetActive(true); break;
            case 2: this.magicalSkills.SetActive(true); this.categoryMagic.gameObject.SetActive(true); break;
            default: this.itemSkills.SetActive(true); this.categoryItems.gameObject.SetActive(true); break;
        }

        SetPage(0);
    }

    public void selectSkillFromSkillSet(SkillSlot skillSlot)
    {
        if (skillSlot != null && skillSlot.ability != null)
        {
            this.selectedAbility = skillSlot.ability;
            this.cursor.setSelectedGameObject(skillSlot.image);
        }
        else
        {
            this.selectedAbility = null;
            this.cursor.setSelectedGameObject(null);
        }
    }

    public void SetPage(int value)
    {
        selectSkillFromSkillSet(null);
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

                if (pagesCount > 0) this.pageText.text = (activeIndex + 1) + "/" + (pagesCount + 1);
                else this.pageText.text = "";
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
                GameObject skills = page.transform.GetChild(page.transform.childCount-1).gameObject;

                for (int ID = 0; ID < skills.transform.childCount; ID++)
                {
                    GameObject slot = skills.transform.GetChild(ID).gameObject;
                    Ability ability = this.skillSet.getSkillByID(slot.GetComponent<SkillSlot>().ID, category);                    
                    slot.GetComponent<SkillSlot>().setSkill(ability);
                }

                if (page.transform.GetSiblingIndex() > 0) page.SetActive(false);
            }
        }
    }        
}
