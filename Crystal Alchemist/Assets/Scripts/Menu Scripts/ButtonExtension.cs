using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{   
    [SerializeField]
    [Required]
    private myCursor cursor;

    private Vector2 scale;
    private Vector2 size;

    [SerializeField]
    public bool setFirstSelected = false;

    private Selectable button;

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
            if (this.button == null) this.button = this.gameObject.GetComponent<Selectable>();

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
    }

    private void init()
    {
        try
        {
            if (this.cursor == null) this.cursor = GameObject.FindWithTag("Cursor").GetComponent<myCursor>();
            if (this.button == null) this.button = this.gameObject.GetComponent<Selectable>();
        }
        catch
        {
            Debug.Log(this.gameObject.name);
        }

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
    
    private void Start()
    {
        init();

        if(this.cursor == null)
        {
            Debug.Log("<color=red>Cursor not found in: "+this.gameObject.name+"</color>");
        }

        /*Debug.Log(this.name + " - "
                    + this.transform.localScale + " - "
                    + this.transform.parent.localScale + " - "
                    + this.transform.parent.parent.localScale + " - "
                    + this.transform.parent.parent.parent.localScale + " - "
                    + WordToScenePoint(this.transform.localPosition));*/

        if(this.setFirstSelected) setFirst();
    }

    public void setCursor(myCursor cursor)
    {
        this.cursor = cursor;
    }

    private void OnEnable()
    {
        init();
        if (this.setFirstSelected) setFirst();
    }

    public void setFirst()
    {
        if(this.button == null) this.button = this.gameObject.GetComponent<Selectable>();

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        setCursor();
    }

    public void OnSelect(BaseEventData eventData)
    {        
        setCursor();
    } 

    private void setCursor()
    {
        this.cursor.setCursorPosition(true, true, this.button, this.size, this.scale);       
    }

}
