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

    private UnityEvent actionYes; 
    private MenuControls lastMainMenu;

    private Costs price;

    [HideInInspector]
    public string dialogText;

    public void setActive(GameObject launcherObject)
    {
        this.priceField.gameObject.SetActive(false);
        MenuDialogBoxLauncher launcher = launcherObject.GetComponent<MenuDialogBoxLauncher>();

        this.YesButton.gameObject.SetActive(false);
        this.NoButton.gameObject.SetActive(false);
        this.OKButton.gameObject.SetActive(false);

        if (launcher != null)
        {
            this.price = launcher.GetPrice();

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

                if (this.price.resourceType != CostType.none)
                {
                    this.priceField.gameObject.SetActive(true);
                    bool canActivateYES = this.priceField.updatePrice(this.player, this.price);
                    this.YesButton.GetComponent<Selectable>().interactable = canActivateYES;
                }

                if (launcher.setYesButtonFirst) this.YesButton.setFirst();
                else this.NoButton.setFirst();
            }

            this.lastMainMenu = launcher.parentMenu;
            if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(false);
        }
    }

    public void Yes()
    {        
        this.closeDialog();
        if (this.actionYes != null)
        {
            this.player.reduceResource(this.price);
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
