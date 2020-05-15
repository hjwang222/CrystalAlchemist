using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextTranslation : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    public string alternativeText;

    private TextMeshProUGUI textMeshField;
    private string originalText;

    private void Awake()
    {
        this.textMeshField = this.GetComponent<TextMeshProUGUI>();
        this.originalText = this.textMeshField.text;
    }

    private void Start() => SettingsEvents.current.OnLanguangeChanged += ChangeLanguageText;

    private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= ChangeLanguageText;

    private void OnEnable() => ChangeLanguageText();   

    public void ChangeLanguageText() => this.textMeshField.text = FormatUtil.getLanguageDialogText(this.originalText, this.alternativeText);

}

