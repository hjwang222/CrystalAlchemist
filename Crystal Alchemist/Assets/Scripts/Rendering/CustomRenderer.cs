using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomRenderer : MonoBehaviour
{
    public Material material;
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool useGlow = false;

    [SerializeField]
    [ShowIf("useGlow")]
    private Color selectColor = Color.white;

    [ShowIf("useGlow")]
    [SerializeField]
    [Range(0,1)]
    private float precision = 0f;

    [ShowIf("useGlow")]
    [SerializeField]
    [ColorUsage(true, true)]
    private Color glowColor = Color.white;

    [SerializeField]
    private bool invert = false;

    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.material = this.GetComponent<SpriteRenderer>().material;
        AddGlow();
    }

    [Button]
    private void Check()
    {
        this.material = this.GetComponent<SpriteRenderer>().sharedMaterial;
        AddGlow();
        InvertColors(this.invert);
    }

    public void SetGlowColor(Color color)
    {
        if (color == null) return;
        this.glowColor = color;

        this.material = this.GetComponent<SpriteRenderer>().material;
        AddGlow();
    }

    public void InvertColors(bool invert)
    {
        this.material.SetFloat("_Invert", invert ? 1f : 0f);
    }

    private void AddGlow()
    {
        this.material.SetFloat("_Use_Glow", this.useGlow ? 1f : 0f);
        this.material.SetFloat("_Precision", this.precision);
        this.material.SetColor("_SelectGlow", this.selectColor);
        this.material.SetColor("_GlowColor", this.glowColor);
    }
}
