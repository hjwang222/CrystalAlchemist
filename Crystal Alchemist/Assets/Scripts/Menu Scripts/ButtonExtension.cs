using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{   
    [SerializeField]
    private GameObject cursor;
    private float offset = 16;
    [SerializeField]
    private Canvas canvas;
    private GameObject lastselect;
    private float scaleFactor = 1f;

    private Vector2 scale;
    private Vector2 size;

    private Vector2 cursorScale;
    private Vector2 cursorSize;

    [SerializeField]
    private bool setFirstSelected = false;

    private void Awake()
    {
        RectTransform rt = (RectTransform)this.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);
        this.scale = rt.lossyScale;

        rt = (RectTransform)this.cursor.transform;
        this.cursorSize = new Vector2(rt.rect.width, rt.rect.height);
        this.cursorScale = rt.lossyScale;

        if (this.canvas != null) this.scaleFactor = this.canvas.scaleFactor;
        //this.scaleFactor = rt.localScale.x;
    }

    private void OnEnable()
    {
        if (this.setFirstSelected)
        {
            EventSystem.current.firstSelectedGameObject = this.gameObject;
            EventSystem.current.SetSelectedGameObject(this.gameObject);
            setCursor(true);
        }
    }    

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
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

    public void setCursor(bool showCursor)
    {
        if (this.cursor != null)
        {
            if (!this.cursor.activeInHierarchy && showCursor) this.cursor.SetActive(true);            
            if(!showCursor) this.cursor.SetActive(false);

            float x_new = (((this.size.x * this.scale.x) + (this.cursorSize.x * this.cursorScale.x)) / 2) - this.offset;
            float y_new = (((this.size.y * this.scale.y) + (this.cursorSize.y * this.cursorScale.y)) / 2) - this.offset;

            this.cursor.transform.position = new Vector2(this.transform.position.x - (x_new), 
                                                         this.transform.position.y + (y_new)); 

        }
    }
}
