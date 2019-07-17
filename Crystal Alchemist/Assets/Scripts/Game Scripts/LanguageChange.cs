using System.Collections;
using System.Collections.Generic;
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
        this.textMeshField.text = Utilities.Format.getLanguageDialogText(this.originalText, this.alternativeText);
    }
}
