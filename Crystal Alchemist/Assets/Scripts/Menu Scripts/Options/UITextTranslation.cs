using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextTranslation : MonoBehaviour
{
    [InfoBox("GameObject Name muss der gleiche sein wie in csv", InfoMessageType.Info)]
    [SerializeField]
    private LocalisationFileType type = LocalisationFileType.menues;

    private TextMeshProUGUI textMeshField;

    private void Awake() => this.textMeshField = this.GetComponent<TextMeshProUGUI>();

    private void Start() => SettingsEvents.current.OnLanguangeChanged += ChangeLanguageText;

    private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= ChangeLanguageText;

    private void OnEnable() => ChangeLanguageText();

    public void ChangeLanguageText()
    {
        string text = FormatUtil.GetLocalisedText(this.gameObject.name, this.type);
        if (text != string.Empty) this.textMeshField.text = text;
    }
}

