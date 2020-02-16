using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEditor;

public class LanguageChange : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    private string alternativeText;

    private string originalText;

    [SerializeField]
    private TextMeshProUGUI textMeshField;

#if UNITY_EDITOR
    [Button]
    public void setComponent()
    {
        this.textMeshField = this.GetComponent<TextMeshProUGUI>();
        this.alternativeText = this.textMeshField.text;
        SignalListener temp = this.gameObject.AddComponent<SignalListener>();
        temp.signal = (SimpleSignal)AssetDatabase.LoadAssetAtPath("Assets/Resources/Scriptable Objects/Signals/languageChangeSignal.asset", typeof(SimpleSignal));
    }
#endif

    private void Awake()
    {
        this.originalText = this.textMeshField.text;
    }

    private void OnEnable()
    {
        ChangeLanguageText();
    }

    public void ChangeLanguageText()
    {
        this.textMeshField.text = CustomUtilities.Format.getLanguageDialogText(this.originalText, this.alternativeText);
    }
}
