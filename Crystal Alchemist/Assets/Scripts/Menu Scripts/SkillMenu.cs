using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillMenu : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private GameObject skillSetSlot;
    [SerializeField]
    private GameObject cursor;
    private SkillType category = SkillType.magical;
    [SerializeField]
    private int maxRows = 2;
    [SerializeField]
    private int maxColumns = 5;
    [SerializeField]
    private float spacing = 10;
    private Vector2 size;
    [SerializeField]
    private TextMeshProUGUI titleField;
    [SerializeField]
    private TextMeshProUGUI skillDetailsName;
    [SerializeField]
    private TextMeshProUGUI skillDetailsStrength;
    [SerializeField]
    private TextMeshProUGUI skillDetailsCost;
    [SerializeField]
    private Image StatusEffects;
    private List<GameObject> slots = new List<GameObject>();
    [SerializeField]
    private SimpleSignal signal;
    [SerializeField]
    private GameObject slotA;
    [SerializeField]
    private GameObject slotB;
    [SerializeField]
    private GameObject slotX;
    [SerializeField]
    private GameObject slotY;
    [SerializeField]
    private GameObject slotRB;


    private StandardSkill selectedSkill;
    private int index = 0;
    private bool isInputPossible = true;
    private int tempChange;
    private bool setSkill = false;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        input();
    }

    private void input()
    {
        if (this.isInputPossible)
        {
            if (Input.GetButtonDown("Inventory"))
            {
                exitMenu();
            }

            if (!this.setSkill)
            {
                int change = (int)Utilities.getInputMenu("Horizontal");
                change += (int)Utilities.getInputMenu("Vertical") * this.maxColumns;

                if (this.tempChange != change)
                {
                    this.tempChange = change;

                    this.index += change;
                    if (this.index < 0) this.index = 0;
                    if (this.index >= this.slots.Count) this.index = this.slots.Count - 1;

                    setCursor();
                    StartCoroutine(temp());
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    exitMenu();
                }
            }
            else
            {
                int change = (int)Utilities.getInputMenu("Horizontal");

                if (this.tempChange != change)
                {
                    this.tempChange = change;

                    this.index += change;
                    if (this.index < 0) this.index = 0;
                    if (this.index >= 4) this.index = 4;

                    switch (this.index)
                    {
                        case 0: this.cursor.transform.position = this.slotA.transform.position; break;
                        case 1: this.cursor.transform.position = this.slotB.transform.position; break;
                        case 2: this.cursor.transform.position = this.slotX.transform.position; break;
                        case 3: this.cursor.transform.position = this.slotY.transform.position; break;
                        case 4: this.cursor.transform.position = this.slotRB.transform.position; break;
                    }
                   
                    StartCoroutine(temp());
                }
            }

            if (Input.GetButtonDown("Submit"))
            {
                if (!this.setSkill)
                {
                    setSkillToButton();
                }
                else
                {
                    setSkillToButtonConfirm(this.index);
                }

                StartCoroutine(temp());
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                goBack();
            }
        }
    }

    public void exitMenu()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void setSkillToButton()
    {
        this.setSkill = true;
        this.index = 0;
        this.cursor.transform.position = this.slotA.transform.position;
    }

    public void getSkillbyButtonPress(SkillSlot slot)
    {
        this.selectedSkill = slot.skill;
    }

    public void setSkillToButtonConfirm(int indexButton)
    {
        switch (indexButton)
        {
            case 0: this.player.AButton = this.selectedSkill; break;
            case 1: this.player.BButton = this.selectedSkill; break;
            case 2: this.player.XButton = this.selectedSkill; break;
            case 3: this.player.YButton = this.selectedSkill; break;
            case 4: this.player.RBButton = this.selectedSkill; break;
        }

        this.signal.Raise();

        goBack();
    }

    private void goBack()
    {
        this.index = 0;
        this.setSkill = false;
        setCursor();
    }

    private IEnumerator temp()
    {
        this.isInputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.isInputPossible = true;
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        this.player.currentState = CharacterState.inDialog;        
        updateUI();
        goBack();
        StartCoroutine(temp());
    }

    private void OnDisable()
    {
        this.player.currentState = CharacterState.idle;
    }

    private void updateUI()
    {        
        this.skillSetSlot.SetActive(true);
        setSlots();
        setCursor();
        this.updateTextField();
        this.skillSetSlot.SetActive(false);
    }

    private void updateTextField()
    {
        switch (this.category)
        {
            case SkillType.physical: this.titleField.text = "Waffen (physisch)"; break;
            case SkillType.magical: this.titleField.text = "Zauber (magisch)"; break;
            case SkillType.item: this.titleField.text = "Gegenstände"; break;
            default: break;
        }        
    }

    private void updateSkillDetails()
    {
        if (this.selectedSkill != null)
        {
            this.skillDetailsName.text = this.selectedSkill.skillName;
            this.skillDetailsStrength.text = "Stärke: " + Mathf.Abs(this.selectedSkill.addLifeTarget) + "";
            this.skillDetailsCost.text = "Kosten: " + Mathf.Abs(this.selectedSkill.addResourceSender) + "";

            if (this.selectedSkill.statusEffects.Count > 0) this.StatusEffects.sprite = this.selectedSkill.statusEffects[0].iconSprite;
            else this.StatusEffects.sprite = null;
        }
        else
        {
            this.skillDetailsName.text = "-";
            this.skillDetailsStrength.text = "Stärke: ";
            this.skillDetailsCost.text = "Kosten: ";

            this.StatusEffects.sprite = null;
        }
    }

    public void changeCategory(int value)
    {
        if ((int)this.category + value >= System.Enum.GetValues(typeof(SkillType)).Length)
        {
            this.category = (SkillType)0;
        }
        else if ((int)this.category + value < 0)
        {
            this.category = (SkillType)System.Enum.GetValues(typeof(SkillType)).Length - 1;
        }
        else
        {
            this.category = (SkillType)((int)this.category + value);
        }

        this.index = 0;

        updateUI();
    }

    private void setSlots()
    {


        if (this.slots.Count > 0)
        {
            for (int i = 0; i < this.slots.Count; i++)
            {
                Destroy(this.slots[i].gameObject);
            }
        }

        this.selectedSkill = null;
        this.slots.Clear();

        RectTransform rt = (RectTransform)this.skillSetSlot.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);

        foreach (StandardSkill skill in this.player.skillSet)
        {
            if (skill.skillType == this.category)
            {
                GameObject temp = Instantiate(this.skillSetSlot, this.transform);
                temp.GetComponent<SkillSlot>().setSkill(skill);
                temp.name = skill.skillName + "-Slot";
                this.slots.Add(temp);
                temp.transform.position = setPosition(this.slots.Count - 1);
            }
        }


    }

    private Vector2 setPosition(int index)
    {
        float posX = (index % this.maxColumns) * (size.x + this.spacing);
        float posY = ((int)(index / this.maxColumns)) * (size.y + this.spacing);

        return new Vector2(this.skillSetSlot.transform.position.x + posX, this.skillSetSlot.transform.position.y + posY);
    }

    private void setCursor()
    {
        if (this.slots.Count > 0)
        {
            GameObject temp = this.slots[this.index];
            this.cursor.transform.position = new Vector2(temp.transform.position.x - (this.size.x / 2), temp.transform.position.y + (this.size.y / 2)); ///????        
            this.selectedSkill = this.slots[this.index].GetComponent<SkillSlot>().skill;
        }
        else
        {
            this.cursor.transform.position = new Vector2(this.skillSetSlot.transform.position.x - (this.size.x / 2), this.skillSetSlot.transform.position.y + (this.size.y / 2)); ///????    
        }

        updateSkillDetails();
    }
}
