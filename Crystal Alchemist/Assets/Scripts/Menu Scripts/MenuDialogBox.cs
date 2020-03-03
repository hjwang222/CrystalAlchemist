using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

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

    private UnityEvent actionYes; 
    private MenuControls lastMainMenu;

    public void setActive(GameObject launcherObject)
    {
        MenuDialogBoxLauncher launcher = launcherObject.GetComponent<MenuDialogBoxLauncher>();

        if (launcher != null)
        {            
            this.isIngameMenu = !launcher.isAdditionalDialog;
            this.child.SetActive(true);
            this.actionYes = launcher.onYes;
            this.textfield.text = launcher.dialogText;

            if (launcher.setYesButtonFirst) this.YesButton.setFirst();
            else this.NoButton.setFirst();
        }

        this.lastMainMenu = launcherObject.GetComponent<MenuControls>();
        if (this.lastMainMenu != null) this.lastMainMenu.enableButtons(false);
    }

    public void Yes()
    {        
        if(this.actionYes != null) this.actionYes.Invoke();
        this.closeDialog();
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
