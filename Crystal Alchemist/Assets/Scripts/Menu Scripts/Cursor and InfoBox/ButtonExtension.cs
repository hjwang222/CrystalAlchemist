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
    private CustomCursor cursor;

    private Vector2 scale;
    private Vector2 size;

    [SerializeField]
    public bool setFirstSelected = false;

    private Selectable button;

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

#if UNITY_EDITOR
    [Button]
    public void setNavigation()
    {
        setButtonNavigation();
    }
#endif

    public void setButtonNavigation()
    {
        if (this.overrideNavigation)
        {
            if (this.button != null)
            {
                Navigation nav = this.button.navigation;
                nav.mode = Navigation.Mode.Explicit;

                nav.selectOnUp = getNextSelectable(this.up);
                nav.selectOnDown = getNextSelectable(this.down);
                nav.selectOnLeft = getNextSelectable(this.left);
                nav.selectOnRight = getNextSelectable(this.right);
                this.button.navigation = nav;
            }
        }

        UnityUtil.SetColors(this.button, Color.white);
    }

    private void Initialize()
    {
        try
        {
            if (this.cursor == null) this.cursor = GameObject.FindWithTag("Cursor").GetComponent<CustomCursor>();
            if (this.button == null) this.button = this.gameObject.GetComponent<Selectable>();
        }
        catch
        {
            Debug.Log(this.gameObject.name);
        }        

        if (this.isInteractable) StartCoroutine(delayCo());
        else UnityUtil.SetInteractable(this.button, false);

        if (!this.isInit) return;

        this.setButtonNavigation();

        RectTransform rt = (RectTransform)this.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);
        this.scale = rt.lossyScale;        
    }

    private Selectable getNextSelectable(List<Selectable> nexts)
    {
        foreach(Selectable next in nexts)
        {
            if (next.gameObject.activeInHierarchy)return next;
        }
        return null;
    }

    private void Start() { Initialize(); this.isInit = false; }

    public void SetCursor(CustomCursor cursor) => this.cursor = cursor;
    
    public CustomCursor GetCursor()
    {
        return this.cursor;
    }

    private void OnEnable() => SetFirst();    

    public void Select()
    {
        if (this.button == null) this.button = this.gameObject.GetComponent<Selectable>();

        if (this.cursor != null
            && this.gameObject.activeInHierarchy)
        {
            this.cursor.gameObject.SetActive(true);

            if (EventSystem.current != null)
            {
                EventSystem.current.firstSelectedGameObject = this.gameObject;
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }

            this.cursor.setCursorPosition(true, false, this.button, this.size, this.scale);
        }
    }

    public void SetFirst()
    {
        if (this.setFirstSelected)
        {
            Initialize();
            Select();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => SetCursor();
    
    public void OnSelect(BaseEventData eventData) => SetCursor();
    
    public void SetCursor()
    {
        if(this.cursor != null) this.cursor.setCursorPosition(true, true, this.button, this.size, this.scale);       
    }

    private IEnumerator delayCo()
    {
        this.button.interactable = false;
        yield return new WaitForSeconds(0.1f);
        this.button.interactable = true;
    }
}

