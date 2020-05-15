using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Collections;

public class MenuDialogBoxLauncher : MonoBehaviour
{
    [BoxGroup("Main")]
    [Required]
    [SerializeField]
    private GameObject parentMenu;

    [BoxGroup("Main")]
    [Required]
    [SerializeField]
    private MenuDialogBoxInfo info;

    [BoxGroup("Main")]
    [SerializeField]
    private DialogBoxType dialogBoxType = DialogBoxType.yesNo;

    [BoxGroup("Actions")]
    [SerializeField]
    [HideLabel]
    private Costs price;

    [BoxGroup("Actions")]
    public UnityEvent actionOnConfirm;

    [BoxGroup("Text")]
    [SerializeField]
    [TextArea]
    private string menuDialogBoxText;

    [BoxGroup("Text")]
    [TextArea]
    [SerializeField]
    private string menuDialogBoxTextEnglish;

    private bool inputPossible;

    private void OnEnable() => StartCoroutine(delayCo());    

    public void ShowDialogBox(MenuDialogBoxEvent action) => RaiseDialogBox(action.action);

    public void ShowDialogBox() => RaiseDialogBox(this.actionOnConfirm);

    private void RaiseDialogBox(UnityEvent action)
    {
        if (!this.inputPossible) return;
        string text = FormatUtil.getLanguageDialogText(this.menuDialogBoxText, this.menuDialogBoxTextEnglish);
        this.info.SetValue(this.actionOnConfirm, this.price, text, this.dialogBoxType, this.parentMenu);
        MenuEvents.current.OpenMenuDialogBox();
    }

    private IEnumerator delayCo()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.3f);
        this.inputPossible = true;
    }
}
