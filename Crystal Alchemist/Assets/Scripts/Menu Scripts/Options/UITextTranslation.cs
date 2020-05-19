using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextTranslation : MonoBehaviour
{
    [InfoBox("GameObject Name muss der gleiche sein wie in csv", InfoMessageType.Info)]
    private TextMeshProUGUI textMeshField;

    private void Awake() => this.textMeshField = this.GetComponent<TextMeshProUGUI>();    

    private void Start() => SettingsEvents.current.OnLanguangeChanged += ChangeLanguageText;

    private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= ChangeLanguageText;

    private void OnEnable() => ChangeLanguageText();   

    public void ChangeLanguageText() => this.textMeshField.text = FormatUtil.GetLocalisedText(this.gameObject.name, LocalisationFileType.menues);

}

