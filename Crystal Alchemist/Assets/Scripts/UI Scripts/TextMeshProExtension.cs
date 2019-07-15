using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProExtension : MonoBehaviour
{
    [Header("Text-Objekte")]
    public TextMeshPro text;
    public TextMeshProUGUI textGUI;

    [Header("Attribute")]
    public Color fontColor;
    public Color outlineColor;
    public float outlineWidth = 0.25f;
    public bool bold = false;

    void Start()
    {
        Utilities.Format.set3DText(this.text, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
        Utilities.Format.set3DText(this.textGUI, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
    }
}
