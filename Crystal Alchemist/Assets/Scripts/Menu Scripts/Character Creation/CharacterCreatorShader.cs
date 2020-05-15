using Sirenix.OdinInspector;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterCreatorShader : MonoBehaviour
{
    [BoxGroup("From")]
    [SerializeField]
    private bool swapColors = true;

    [BoxGroup("From")]
    [SerializeField]
    private bool glow = true;

    [BoxGroup("From")]
    [ShowIf("swapColors")]
    [SerializeField]
    private Color baseColor = Color.green;

    [BoxGroup("From")]
    [ShowIf("swapColors")]
    [SerializeField]
    private Color lightColor = Color.green;

    [BoxGroup("From")]
    [ShowIf("swapColors")]
    [SerializeField]
    private Color shadeColor = Color.green;

    [BoxGroup("From")]
    [ShowIf("swapColors")]
    [SerializeField]
    private Color lineColor = Color.green;

    [BoxGroup("From")]
    [ShowIf("glow")]
    [SerializeField]
    private Color highlight = Color.green;

    [BoxGroup("To")]
    [ShowIf("swapColors")]
    [SerializeField]
    private Color toColor = Color.red;

    [BoxGroup("To")]
    [ShowIf("glow")]
    [SerializeField]
    [ColorUsageAttribute(true, true)]
    private Color toHighlight = Color.red;

    [BoxGroup("To")]
    [SerializeField]
    private bool invertColors = false;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.material = this.spriteRenderer.material;
    }

    [Button]
    private void Temp()
    {
        this.material.SetInt("_Swap", Convert.ToInt32(this.swapColors));
        this.material.SetInt("_Invert", Convert.ToInt32(this.invertColors));
        this.material.SetInt("_Glow", Convert.ToInt32(this.glow));

        this.material.SetColor("_FromColor1", this.baseColor);
        this.material.SetColor("_FromColor2", this.shadeColor);
        this.material.SetColor("_FromColor3", this.lightColor);
        this.material.SetColor("_FromColor4", this.lineColor);
        this.material.SetColor("_FromHighlight", this.highlight);

        this.material.SetColor("_ToColor", this.toColor);
        this.material.SetColor("_ToHighlight", this.toHighlight);
    }

}
