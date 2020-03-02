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
    public UnityEvent onNo;

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
        this.raiseDialogBox(this.menuDialogBoxText, this.menuDialogBoxTextEnglish, this.onYes, this.onNo);
    }

    public void raiseDialogBox(string text, string textEnglish, UnityEvent yes, UnityEvent no)
    {
        this.dialogText = CustomUtilities.Format.getLanguageDialogText(text, textEnglish);
        this.signal.Raise(this.gameObject);
    }
}
