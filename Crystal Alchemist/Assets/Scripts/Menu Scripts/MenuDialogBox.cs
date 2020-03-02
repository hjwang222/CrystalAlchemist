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
    private UnityEvent actionNo;

    public void setActive(GameObject launcherObject)
    {
        MenuDialogBoxLauncher launcher = launcherObject.GetComponent<MenuDialogBoxLauncher>();

        if (launcher != null)
        {
            this.child.SetActive(true);
            this.isIngameMenu = !launcher.isAdditionalDialog;
            this.actionYes = launcher.onYes;
            this.actionNo = launcher.onNo;
            this.textfield.text = launcher.dialogText;

            if (launcher.setYesButtonFirst) this.YesButton.setFirst();
            else this.NoButton.setFirst();
        }
    }

    public void Yes()
    {        
        if(this.actionYes != null) this.actionYes.Invoke();
        this.closeDialog();
    }

    public void No()
    {
        if (this.actionNo != null) this.actionNo.Invoke();
        this.closeDialog();
    }

    private void closeDialog()
    {
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

    public void setNo(UnityEvent action)
    {
        this.actionNo = action;
    }
}
