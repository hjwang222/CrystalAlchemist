using UnityEngine;
using TMPro;

public class LanguageChange : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    private string alternativeText;

    private string originalText;

    [SerializeField]
    private TextMeshProUGUI textMeshField;

    private void Awake() => this.originalText = this.textMeshField.text;

    private void Start() => SettingsEvents.current.OnLanguangeChanged += ChangeLanguageText;

    private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= ChangeLanguageText;

    private void OnEnable() => ChangeLanguageText();   

    public void ChangeLanguageText() => this.textMeshField.text = FormatUtil.getLanguageDialogText(this.originalText, this.alternativeText);

}

