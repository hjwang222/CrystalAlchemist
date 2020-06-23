using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class TextMeshProExtension : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField]
    private Color fontColor;
    [SerializeField]
    private Color outlineColor;
    [SerializeField]
    private float outlineWidth = 0.25f;
    [SerializeField]
    private bool bold = false;

    private void Start() => UpdateTextMesh();    

    private void UpdateTextMesh()
    {
        TextMeshPro text = this.GetComponent<TextMeshPro>();
        TextMeshProUGUI textGUI = this.GetComponent<TextMeshProUGUI>();

        if (text != null) FormatUtil.set3DText(text, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
        if (textGUI != null) FormatUtil.set3DText(textGUI, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
    }
}
