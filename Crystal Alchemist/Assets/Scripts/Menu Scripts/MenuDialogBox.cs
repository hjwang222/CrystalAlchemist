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
    private SimpleSignal enableButtonSignal;

    private UnityEvent actionYes;
    private UnityEvent actionNo;

    public void setActive(bool isadditionalDialog)
    {
        this.disableMenuControlsScript = isadditionalDialog;
        this.child.SetActive(true);
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
        this.enableButtonSignal.Raise();
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
