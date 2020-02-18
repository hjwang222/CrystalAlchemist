using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

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

    private void init()
    {
        if (this.cursor == null) this.cursor = GameObject.FindWithTag("Cursor").GetComponent<myCursor>();

        RectTransform rt = (RectTransform)this.transform;
        this.size = new Vector2(rt.rect.width, rt.rect.height);
        this.scale = rt.lossyScale;
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

        setFirst();
    }

    public void setCursor(myCursor cursor)
    {
        this.cursor = cursor;
    }

    private void OnEnable()
    {
        init();
        setFirst();
    }

    public void setFirst()
    {
        if(this.button == null) this.button = this.gameObject.GetComponent<Selectable>();

        if (this.cursor != null 
            && this.setFirstSelected  
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
