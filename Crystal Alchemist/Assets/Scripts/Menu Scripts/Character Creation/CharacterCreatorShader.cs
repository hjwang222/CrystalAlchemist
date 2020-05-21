using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterCreatorShader : MonoBehaviour
{
    [System.Serializable]
    public struct ColorSwap
    {
        public ColorPaletteBase from;
        public ColorPaletteOverride to;
    }

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [BoxGroup("Buttons")]
    [SerializeField]
    private bool invert;

    [SerializeField]
    private List<ColorSwap> colorPalettes = new List<ColorSwap>();

    private int maxColorCount = 7;

    private void Awake()
    {
        Temp();
    }

    private void Temp()
    {        
        this.spriteRenderer.material.SetInt("_Invert", Convert.ToInt32(this.invert));

        SwapColors();

    }

    public void SwapColors()
    {
        int count = 1;

        for(int i = 1; i <= this.maxColorCount; i++)
        {
            this.spriteRenderer.material.SetColor("_Color_" + i, Color.black);
            this.spriteRenderer.material.SetColor("_New_Color_" + i, Color.black);
        }

        this.spriteRenderer.material.SetColor("_Color_Highlight", Color.black);
        this.spriteRenderer.material.SetColor("_New_Highlight", Color.black);

        foreach (ColorSwap swap in this.colorPalettes)
        {
            if (swap.from == null || swap.to == null) break;

            for(int i = 0; i < swap.from.colors.Count; i++)
            {
                Color from = swap.from.colors[i];
                if (i >= swap.to.colors.Count) break;

                Color to = swap.to.colors[i];

                this.spriteRenderer.material.SetColor("_Color_"+count, from);
                this.spriteRenderer.material.SetColor("_New_Color_" + count, to);
                count++;
            }

            if (swap.to.addGlow)
            {
                this.spriteRenderer.material.SetColor("_Color_Highlight", swap.from.selectHighlight);
                this.spriteRenderer.material.SetColor("_New_Highlight", swap.to.highlight);
            }
            
            if (count > this.maxColorCount) break;
        }
    }

}
