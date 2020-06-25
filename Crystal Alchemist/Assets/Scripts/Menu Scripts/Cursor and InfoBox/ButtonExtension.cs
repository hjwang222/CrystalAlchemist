using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;

public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField]
    [Required]
    public CustomCursor cursor;

    [SerializeField]
    public bool selectFirst = false;

    [SerializeField]
    private bool isInteractable = true;

    [SerializeField]
    private bool overrideNavigation = false;

    [ShowIf("overrideNavigation", true)]
    [SerializeField]
    private List<Selectable> up = new List<Selectable>();

    [ShowIf("overrideNavigation", true)]
    [SerializeField]
    private List<Selectable> down = new List<Selectable>();

    [ShowIf("overrideNavigation", true)]
    [SerializeField]
    private List<Selectable> left = new List<Selectable>();

    [ShowIf("overrideNavigation", true)]
    [SerializeField]
    private List<Selectable> right = new List<Selectable>();

    private bool isInit = true;
    private Selectable button;

    private void OnEnable()
    {
        if (isInit) Initialize();
        if (selectFirst) Select();
    }

    private void Initialize()
    {
        this.button = this.GetComponent<Selectable>();
        this.setButtonNavigation();
        UnityUtil.SetColors(this.button, Color.white);
        this.isInit = false;
    }

    [Button]
    public void setButtonNavigation()
    {
        if (this.overrideNavigation)
        {
            if (this.button != null)
            {
                Navigation nav = this.button.navigation;
                nav.mode = Navigation.Mode.Explicit;

                nav.selectOnUp = GetNavigationNeighbor(this.up);
                nav.selectOnDown = GetNavigationNeighbor(this.down);
                nav.selectOnLeft = GetNavigationNeighbor(this.left);
                nav.selectOnRight = GetNavigationNeighbor(this.right);
                this.button.navigation = nav;
            }
        }
    }

    private Selectable GetNavigationNeighbor(List<Selectable> nexts)
    {
        foreach (Selectable next in nexts)
        {
            if (next.gameObject.activeInHierarchy) return next;
        }
        return null;
    }


    public void Select()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.firstSelectedGameObject = this.gameObject;
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

        SetCursor();
    }

    public void OnPointerEnter(PointerEventData eventData) => Select();

    public void OnSelect(BaseEventData eventData) => SetCursor();

    public void SetCursor()
    {
        this.cursor.Select(!this.isInit, this.button);
    }

    public void ReSelect()
    {
        if (this.selectFirst) Select();
    }

    public void SetAsFirst() => this.selectFirst = true;
}

