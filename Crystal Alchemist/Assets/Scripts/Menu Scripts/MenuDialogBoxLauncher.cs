using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class MenuDialogBoxLauncher : MonoBehaviour
{
    [BoxGroup("DialogBox")]
    [SerializeField]
    [Required]
    private GameObjectSignal signal;

    [BoxGroup("DialogBox")]
    [Required]
    public MenuControls parentMenu;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private bool useActionOnConfirm = true;

    [ShowIf("useActionOnConfirm")]
    [BoxGroup("DialogBox")]
    public UnityEvent actionOnConfirm;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [TextArea]
    private string menuDialogBoxText;

    [BoxGroup("DialogBox")]
    [TextArea]
    [SerializeField]
    private string menuDialogBoxTextEnglish;

    [BoxGroup("DialogBox")]
    public DialogBoxType dialogBoxType = DialogBoxType.yesNo;

    private Costs price;
    private string dialogText;

    public void SetPrice(Costs price)
    {
        this.price = price;
    }

    public Costs GetPrice()
    {
        return this.price;
    }

    public string GetText()
    {
        return this.dialogText;
    }

    public void raiseDialogBox()
    {
        this.raiseDialogBox(this.menuDialogBoxText, this.menuDialogBoxTextEnglish, this.actionOnConfirm);
    }

    public void raiseDialogBox(MenuDialogBoxEvent dialogBoxEvent)
    {
        this.actionOnConfirm = dialogBoxEvent.action;
        this.raiseDialogBox(this.menuDialogBoxText, this.menuDialogBoxTextEnglish, this.actionOnConfirm);
    }

    private void raiseDialogBox(string text, string textEnglish, UnityEvent yes)
    {
        this.dialogText = FormatUtil.getLanguageDialogText(text, textEnglish);
        this.signal.Raise(this.gameObject);
    }
}
