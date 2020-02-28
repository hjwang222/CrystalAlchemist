using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class MenuDialogBoxLauncher : MonoBehaviour
{
    [BoxGroup("DialogBox")]
    [SerializeField]
    [Required]
    private StringSignal menuDialogBoxSignal;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [Required]
    private BoolSignal showMenuDialogBoxSignal;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ActionSignal actionYes;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private ActionSignal actionNo;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private UnityEvent onYes;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private UnityEvent onNo;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [TextArea]
    private string menuDialogBoxText;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [TextArea]
    private string menuDialogBoxTextEnglish;

    [BoxGroup("DialogBox")]
    [SerializeField]
    private bool isAdditionalDialog = true;

    public void raise()
    {
        this.raiseDialogBox(this.menuDialogBoxText, this.menuDialogBoxTextEnglish, this.onYes, this.onNo);
    }

    public void raiseDialogBox(string text, string textEnglish, UnityEvent yes, UnityEvent no)
    {
        this.showMenuDialogBoxSignal.Raise(this.isAdditionalDialog);
        string dialogtext = CustomUtilities.Format.getLanguageDialogText(text, textEnglish);
        this.menuDialogBoxSignal.Raise(dialogtext);
        this.actionYes.Raise(yes);
        this.actionNo.Raise(no);
    }
}
