using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private Vector2 size;

    [SerializeField]
    private GameObject cursor;
    private GameObject lastselect;


    [SerializeField]
    private bool setFirstSelected = false;

    private void Awake()
    {
        RectTransform rt = (RectTransform)this.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);
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

    private void OnDisable()
    {
        setCursor(false);
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

            this.cursor.transform.position
            = new Vector2(this.transform.position.x - (this.size.x / 2), this.transform.position.y + (this.size.y / 2)); 

        }
    }
}
