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
    public UnityEvent onYes;

    [BoxGroup("DialogBox")]
    [SerializeField]
    [TextArea]
    private string menuDialogBoxText;

    [BoxGroup("DialogBox")]
    [TextArea]
    [SerializeField]
    private string menuDialogBoxTextEnglish;

    [BoxGroup("DialogBox")]
    public bool isAdditionalDialog = true;

    [BoxGroup("DialogBox")]
    public bool setYesButtonFirst = true;

    [HideInInspector]
    public string dialogText;

    public void raiseDialogBox()
    {
        this.raiseDialogBox(this.menuDialogBoxText, this.menuDialogBoxTextEnglish, this.onYes);
    }

    public void raiseDialogBox(MenuDialogBoxEvent dialogBoxEvent)
    {
        this.onYes = dialogBoxEvent.action;
        this.raiseDialogBox(this.menuDialogBoxText, this.menuDialogBoxTextEnglish, this.onYes);
    }

    private void raiseDialogBox(string text, string textEnglish, UnityEvent yes)
    {
        this.dialogText = CustomUtilities.Format.getLanguageDialogText(text, textEnglish);
        this.signal.Raise(this.gameObject);
    }
}
