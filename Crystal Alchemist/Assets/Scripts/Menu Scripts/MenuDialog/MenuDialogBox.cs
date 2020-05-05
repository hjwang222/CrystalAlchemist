using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public enum DialogBoxType
{
    yesNo,
    ok
}

public class MenuDialogBox : MonoBehaviour
{
    [BoxGroup("DialogBox")]
    [SerializeField]
    private TextMeshProUGUI textfield;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ButtonExtension YesButton;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ButtonExtension NoButton;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ButtonExtension OKButton;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private MiniGamePrice priceField;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private PlayerInventory inventory;

    [SerializeField]
    private GameObject child;

    [SerializeField]
    private MenuControls controls;

    private UnityEvent OnConfirm; 
    private MenuControls lastMainMenu;
    private Costs price;

    public void Start()
    {
        GameEvents.current.OnMenuDialogBox += Initialize;
    }

    public void OnDestroy()
    {
        GameEvents.current.OnMenuDialogBox -= Initialize;
    }

    private void init()
    {
        this.child.SetActive(true);
        this.YesButton.gameObject.SetActive(false);
        this.NoButton.gameObject.SetActive(false);
        this.OKButton.gameObject.SetActive(false);
        this.price = null;
        this.priceField.gameObject.SetActive(false);
        this.YesButton.GetComponent<Selectable>().interactable = true;        
    }

    private void Initialize(UnityEvent OnConfirm, Costs cost, string text, DialogBoxType type, MenuControls parent)
    {
        init();
        this.OnConfirm = OnConfirm;
        this.lastMainMenu = parent;
        this.textfield.text = text;

        if (type == DialogBoxType.ok) this.OKButton.gameObject.SetActive(true);        
        else
        {
            this.YesButton.gameObject.SetActive(true);
            this.NoButton.gameObject.SetActive(true);
            setPrice(cost);
        }
        
        if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(false);
    }

    private void setPrice(Costs cost)
    {
        this.price = cost;
        if (this.price != null && this.price.resourceType != CostType.none)
        {
            this.priceField.gameObject.SetActive(true);
            this.YesButton.GetComponent<Selectable>().interactable = this.priceField.updatePrice(this.inventory, this.price);
        }
    }

    public void Yes()
    {        
        this.closeDialog();
        if (this.OnConfirm != null)
        {
            GameEvents.current.DoReduce(this.price);
            this.OnConfirm.Invoke();
        }
    }

    public void No() => this.closeDialog();    

    private void closeDialog()
    {
        if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(true);   
        this.controls.ExitMenu();
    }
}
