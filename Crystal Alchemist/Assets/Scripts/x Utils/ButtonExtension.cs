using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private EventSystem eventSystem;
    private Vector2 size;

    [Required]
    [SerializeField]
    private GameObject cursor;

    private void Start()
    {
        RectTransform rt = (RectTransform)this.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);
    }

    private void OnEnable()
    {
        this.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        setCursor();
    }

    public void OnSelect(BaseEventData eventData)
    {        
        setCursor();
    }

    public void setCursor()
    {
        this.cursor.transform.position = new Vector2(this.transform.position.x - (this.size.x / 2), this.transform.position.y + (this.size.y / 2)); ///????  
    }
}
