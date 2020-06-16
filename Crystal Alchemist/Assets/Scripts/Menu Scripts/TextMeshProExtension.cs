using UnityEngine;
using TMPro;

public class TextMeshProExtension : MonoBehaviour
{
    [Header("Text-Objekte")]
    [SerializeField]
    private TextMeshPro text;
    [SerializeField]
    private TextMeshProUGUI textGUI;

    [Header("Attribute")]
    [SerializeField]
    private Color fontColor;
    [SerializeField]
    private Color outlineColor;
    [SerializeField]
    private float outlineWidth = 0.25f;
    [SerializeField]
    private bool bold = false;

    private void Start()
    {
        FormatUtil.set3DText(this.text, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
        FormatUtil.set3DText(this.textGUI, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
    }
}
