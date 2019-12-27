using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{   
    [SerializeField]
    private myCursor cursor;
    private float offset = 16;

    private Vector2 scale;
    private Vector2 size;

    private Vector2 cursorScale;
    private Vector2 cursorSize;

    [SerializeField]
    private bool setFirstSelected = false;
    private Button button;

    private int distance = 400;

    /*
    private void Awake()
    {
        //this.cursor.gameObject.SetActive(false);
    }
    */
    private void Start()
    {
        if(this.cursor == null)
        {
            Debug.Log("<color=red>Cursor not found in: "+this.gameObject.name+"</color>");
        }

        RectTransform rt = (RectTransform)this.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);
        this.scale = rt.lossyScale;

        rt = (RectTransform)this.cursor.transform;
        this.cursorSize = new Vector2(rt.rect.width, rt.rect.height);
        this.cursorScale = rt.lossyScale;

        this.button = this.gameObject.GetComponent<Button>();

        /*Debug.Log(this.name + " - "
                    + this.transform.localScale + " - "
                    + this.transform.parent.localScale + " - "
                    + this.transform.parent.parent.localScale + " - "
                    + this.transform.parent.parent.parent.localScale + " - "
                    + WordToScenePoint(this.transform.localPosition));*/

        setFirst();

        //Cursor.visible = false;
    }

    public void setCursor(myCursor cursor)
    {
        this.cursor = cursor;
    }

    private void OnEnable()
    {
        setFirst();
    }

    public void setFirst()
    {
        if (this.cursor != null && this.setFirstSelected)
        {
            this.cursor.gameObject.SetActive(true);

            EventSystem.current.firstSelectedGameObject = this.gameObject;
            EventSystem.current.SetSelectedGameObject(this.gameObject);
            setCursor(true, false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        setCursor(true);
    }

    public void OnSelect(BaseEventData eventData)
    {        
        setCursor(true);
    }

    private void setCursor(bool showCursor, bool playEffect)
    {       
        if (this.cursor != null)
        {
            if (!this.cursor.gameObject.activeInHierarchy && showCursor) this.cursor.gameObject.SetActive(true);            
            if(!showCursor) this.cursor.gameObject.SetActive(false);

            float x_new = (((this.size.x * this.scale.x) + (this.cursorSize.x * this.cursorScale.x)) / 2) - this.offset;
            float y_new = (((this.size.y * this.scale.y) + (this.cursorSize.y * this.cursorScale.y)) / 2) - this.offset;

            this.cursor.transform.position = new Vector2(this.transform.position.x - (x_new), 
                                                         this.transform.position.y + (y_new));

            setInfoBox();

            //Debug.Log("Button: "+this.size + " - " + this.scale);
            //Debug.Log("Cursor: "+this.cursorSize + " - " + this.cursorScale);

            if (playEffect) this.cursor.GetComponent<myCursor>().playSoundEffect();
        }
    }

    private void setInfoBox()
    {
        if (this.button != null)
        {
            this.button.Select();

            if (this.cursor.infoBox != null)
            {
                if (this.cursor.transform.localPosition.x < 0)
                {
                    //right
                    RectTransform panelRectTransform = (RectTransform)this.cursor.infoBox.transform;
                    panelRectTransform.anchorMin = new Vector2(1, 0.5f);
                    panelRectTransform.anchorMax = new Vector2(1, 0.5f);
                    panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    panelRectTransform.position = new Vector3(Screen.width - (this.cursorScale.x * distance), (Screen.height / 2) + 40, 0);
                }
                else
                {
                    //left
                    RectTransform panelRectTransform = (RectTransform)this.cursor.infoBox.transform;
                    panelRectTransform.anchorMin = new Vector2(0, 0.5f);
                    panelRectTransform.anchorMax = new Vector2(0, 0.5f);
                    panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    panelRectTransform.position = new Vector3((this.cursorScale.x * distance), (Screen.height / 2) + 40, 0);
                }

                ItemUI itemUI = this.button.gameObject.GetComponent<ItemUI>();
                SkillSlot skillSlot = this.button.gameObject.GetComponent<SkillSlot>();
                SkillMenuActiveSlots activeSlot = this.button.gameObject.GetComponent<SkillMenuActiveSlots>();
                CharacterAttributeStats attributesStat = this.button.gameObject.GetComponent<CharacterAttributeStats>();

                if (itemUI != null && itemUI.getItem() != null)
                {
                    this.cursor.infoBox.Show(itemUI.getItem());
                }
                else if (skillSlot != null && skillSlot.skill != null)
                {
                    this.cursor.infoBox.Show(skillSlot.skill);
                }
                else if (activeSlot != null && activeSlot.skill != null)
                {
                    this.cursor.infoBox.Show(activeSlot.skill);
                }
                else if (attributesStat != null)
                {
                    this.cursor.infoBox.Show(attributesStat);
                }
                else
                {
                    this.cursor.infoBox.Hide();
                }
            }
        }
    }

    private void setCursor(bool showCursor)
    {
        setCursor(showCursor, true);       
    }
}
