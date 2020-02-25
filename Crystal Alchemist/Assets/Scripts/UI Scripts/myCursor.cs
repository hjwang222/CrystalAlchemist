using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class myCursor : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundEffect;
    //private bool isPlaying = false;

    public InfoBox infoBox;

    [SerializeField]
    private Image image;

    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private GameObject cursorSelected;

    private Vector2 cursorScale;
    private Vector2 cursorSize;
    private float offset = 16;
    private int distance = 400;

    public GameObject selectedObject;

    private void Start()
    {
        this.image.sprite = null;
        this.cursorSelected.SetActive(false);
        this.cursor.SetActive(true);

        this.getCursorSize();
    }

    private void getCursorSize()
    {
        RectTransform rt = (RectTransform)this.cursor.transform;
        this.cursorSize = new Vector2(rt.rect.width, rt.rect.height);
        this.cursorScale = rt.lossyScale;
    }

    private void OnEnable()
    {
        this.getCursorSize();
        //this.isPlaying = false;
    }

    private void OnDisable()
    {
        //this.isPlaying = false;
    }

    public void setCursorPosition(bool showCursor, bool playEffect, Selectable button, Vector2 size, Vector2 scale)
    {
        if (button != null && this.selectedObject != button.gameObject)
        {
            this.selectedObject = button.gameObject;

            float x_new = (((size.x * scale.x) + (this.cursorSize.x * this.cursorScale.x)) / 2) - this.offset;
            float y_new = (((size.y * scale.y) + (this.cursorSize.y * this.cursorScale.y)) / 2) - this.offset;


            this.transform.position = new Vector2(button.gameObject.transform.position.x - (x_new),
                                                  button.gameObject.transform.position.y + (y_new));

            setInfoBox(button);

            //Debug.Log("Button: "+this.size + " - " + this.scale);
            //Debug.Log("Cursor: "+this.cursorSize + " - " + this.cursorScale);

            if (playEffect) this.GetComponent<myCursor>().playSoundEffect();
        }
    }

    private void setInfoBox(Selectable button)
    {
        if (button != null)
        {
            button.Select();

            if (this.infoBox != null)
            {
                if (this.transform.localPosition.x < 0)
                {
                    //right
                    RectTransform panelRectTransform = (RectTransform)this.infoBox.transform;
                    panelRectTransform.anchorMin = new Vector2(1, 0.5f);
                    panelRectTransform.anchorMax = new Vector2(1, 0.5f);
                    panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    panelRectTransform.position = new Vector3(Screen.width - (this.cursorScale.x * this.distance), (Screen.height / 2) + 40, 0);
                }
                else
                {
                    //left
                    RectTransform panelRectTransform = (RectTransform)this.infoBox.transform;
                    panelRectTransform.anchorMin = new Vector2(0, 0.5f);
                    panelRectTransform.anchorMax = new Vector2(0, 0.5f);
                    panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    panelRectTransform.position = new Vector3((this.cursorScale.x * this.distance), (Screen.height / 2) + 40, 0);
                }

                ItemUI itemUI = button.gameObject.GetComponent<ItemUI>();
                SkillSlot skillSlot = button.gameObject.GetComponent<SkillSlot>();
                SkillMenuActiveSlots activeSlot = button.gameObject.GetComponent<SkillMenuActiveSlots>();
                CharacterAttributeStats attributesStat = button.gameObject.GetComponent<CharacterAttributeStats>();
                MapPagePoint mapPoint = button.gameObject.GetComponent<MapPagePoint>();

                if (itemUI != null && itemUI.getItem() != null)
                {
                    this.infoBox.Show(itemUI.getItem());
                }
                else if (skillSlot != null && skillSlot.skill != null)
                {
                    this.infoBox.Show(skillSlot.skill);
                }
                else if (activeSlot != null && activeSlot.skill != null)
                {
                    this.infoBox.Show(activeSlot.skill);
                }
                else if (attributesStat != null)
                {
                    this.infoBox.Show(attributesStat);
                }
                else if (mapPoint != null)
                {
                    this.infoBox.Show(mapPoint);
                }
                else
                {
                    this.infoBox.Hide();
                }
            }
        }
    }





    /*
    public void setCursor(Vector2 size, Vector2 scale, Button button)
    {
        this.cursor.gameObject.SetActive(true);

        if (this.selectedObject != this.gameObject)
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.firstSelectedGameObject = this.gameObject;
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }

            this.selectedObject = this.gameObject;
            setCursor(true, false, size, scale, button);
        }
    }

    public void setCursor(bool showCursor, bool playEffect, Vector2 size, Vector2 scale, Button button)
    {
        if (this.cursor != null)
        {
            if (!this.cursor.gameObject.activeInHierarchy && showCursor) this.cursor.gameObject.SetActive(true);
            if (!showCursor) this.cursor.gameObject.SetActive(false);

            float x_new = (((size.x * scale.x) + (this.cursorSize.x * this.cursorScale.x)) / 2) - this.offset;
            float y_new = (((size.y * scale.y) + (this.cursorSize.y * this.cursorScale.y)) / 2) - this.offset;

            this.cursor.transform.position = new Vector2(this.transform.position.x - (x_new),
                                                         this.transform.position.y + (y_new));

            setInfoBox(button);

            //Debug.Log("Button: "+this.size + " - " + this.scale);
            //Debug.Log("Cursor: "+this.cursorSize + " - " + this.cursorScale);

            if (playEffect) this.cursor.GetComponent<myCursor>().playSoundEffect();
        }
    }

    private void setInfoBox(Button button)
    {
        if (button != null)
        {
            button.Select();

            if (this.infoBox != null)
            {
                if (this.cursor.transform.localPosition.x < 0)
                {
                    //right
                    RectTransform panelRectTransform = (RectTransform)this.infoBox.transform;
                    panelRectTransform.anchorMin = new Vector2(1, 0.5f);
                    panelRectTransform.anchorMax = new Vector2(1, 0.5f);
                    panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    panelRectTransform.position = new Vector3(Screen.width - (this.cursorScale.x * distance), (Screen.height / 2) + 40, 0);
                }
                else
                {
                    //left
                    RectTransform panelRectTransform = (RectTransform)this.infoBox.transform;
                    panelRectTransform.anchorMin = new Vector2(0, 0.5f);
                    panelRectTransform.anchorMax = new Vector2(0, 0.5f);
                    panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    panelRectTransform.position = new Vector3((this.cursorScale.x * distance), (Screen.height / 2) + 40, 0);
                }

                ItemUI itemUI = button.gameObject.GetComponent<ItemUI>();
                SkillSlot skillSlot = button.gameObject.GetComponent<SkillSlot>();
                SkillMenuActiveSlots activeSlot = button.gameObject.GetComponent<SkillMenuActiveSlots>();
                CharacterAttributeStats attributesStat = button.gameObject.GetComponent<CharacterAttributeStats>();
                MapPagePoint mapPoint = button.gameObject.GetComponent<MapPagePoint>();

                if (itemUI != null && itemUI.getItem() != null)
                {
                    this.infoBox.Show(itemUI.getItem());
                }
                else if (skillSlot != null && skillSlot.skill != null)
                {
                    this.infoBox.Show(skillSlot.skill);
                }
                else if (activeSlot != null && activeSlot.skill != null)
                {
                    this.infoBox.Show(activeSlot.skill);
                }
                else if (attributesStat != null)
                {
                    this.infoBox.Show(attributesStat);
                }
                else if (mapPoint != null)
                {
                    this.infoBox.Show(mapPoint);
                }
                else
                {
                    this.infoBox.Hide();
                }
            }
        }
    }

    */

    public void setSelectedGameObject(Image image)
    {
        if (image == null)
        {
            this.image.sprite = null;
            this.cursorSelected.SetActive(false);
            this.cursor.SetActive(true);
        }
        else
        {
            this.image.sprite = image.sprite;
            this.cursorSelected.SetActive(true);
            this.cursor.SetActive(false);
        }
    }

    public void playSoundEffect()
    {
        CustomUtilities.Audio.playSoundEffect(this.soundEffect);
    }
}
