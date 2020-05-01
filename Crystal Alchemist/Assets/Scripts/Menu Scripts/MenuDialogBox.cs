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

public class MenuDialogBox : MenuControls
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

    private UnityEvent actionYes; 
    private MenuControls lastMainMenu;

    private Costs price;

    [HideInInspector]
    public string dialogText;

    private void init()
    {
        this.YesButton.gameObject.SetActive(false);
        this.NoButton.gameObject.SetActive(false);
        this.OKButton.gameObject.SetActive(false);
        this.price = null;
        this.priceField.gameObject.SetActive(false);
        this.YesButton.GetComponent<Selectable>().interactable = true;
    }

    public void setActive(GameObject launcherObject)
    {
        MenuDialogBoxLauncher launcher = launcherObject.GetComponent<MenuDialogBoxLauncher>();

        init();

        if (launcher != null)
        {
            this.child.SetActive(true);
            this.actionYes = launcher.actionOnConfirm;
            this.textfield.text = launcher.GetText();

            if(launcher.dialogBoxType == DialogBoxType.ok)
            {
                this.OKButton.gameObject.SetActive(true);
            }
            else
            {
                this.YesButton.gameObject.SetActive(true);
                this.NoButton.gameObject.SetActive(true);

                setPrice(launcher);
            }

            this.lastMainMenu = launcher.parentMenu;
            if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(false);
        }
    }

    private void setPrice(MenuDialogBoxLauncher launcher)
    {
        this.price = launcher.GetPrice();

        if (this.price != null && this.price.resourceType != CostType.none)
        {
            this.priceField.gameObject.SetActive(true);
            this.YesButton.GetComponent<Selectable>().interactable = this.priceField.updatePrice(this.inventory, this.price);
        }
    }

    public void Yes()
    {        
        this.closeDialog();
        if (this.actionYes != null)
        {
            GameEvents.current.DoReduce(this.price);
            this.actionYes.Invoke();
        }
    }

    public void No()
    {
        this.closeDialog();
    }

    private void closeDialog()
    {
        if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(true);   
        this.exitMenu();
    }

    public void SetText(string text)
    {
        this.textfield.text = text;
    }

    public void setYes(UnityEvent action)
    {
        this.actionYes = action;
    }
}
