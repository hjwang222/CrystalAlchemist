using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalisationUI : MonoBehaviour
{
    private TextMeshProUGUI textField;

    [InfoBox("Experimental! Use UI Text Translation instead!",InfoMessageType.Warning)]
    [SerializeField]
    private string key;

    [SerializeField]
    private LocalisationFileType type = LocalisationFileType.menues;

    private void Start()
    {
        this.textField = GetComponent<TextMeshProUGUI>();
        SettingsEvents.current.OnLanguangeChanged += ChangeLanguageText;
        ChangeLanguageText();
    }

    private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= ChangeLanguageText;

    public void ChangeLanguageText()
    {
        this.textField.text = LocalisationSystem.GetLocalisedValue(this.key, this.type);
    }
}
