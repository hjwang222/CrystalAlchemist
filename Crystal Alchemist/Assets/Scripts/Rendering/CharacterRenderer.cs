using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterRenderer : MonoBehaviour
{
    public enum ColorChangeMode
    {
        colorgroups,
        colorpalettes
    }

    [System.Serializable]
    public struct ColorPalettes
    {
        public ColorPaletteBase from;
        public ColorPaletteNew to;
    }

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool invert;
    private bool useTint;

    [BoxGroup("Color change")]
    [SerializeField]
    private bool swapColors;

    [BoxGroup("Color change")]
    [SerializeField]
    private bool addGlow;

    [BoxGroup("Color change")]
    [SerializeField]
    private ColorChangeMode mode = ColorChangeMode.colorgroups;

    [BoxGroup("Color change")]
    [SerializeField]
    private List<ColorPalettes> palettes = new List<ColorPalettes>();
    
    private int maxAmount = 8;
    private Material material;

    [Button]
    private void Test()
    {
        this.material = this.spriteRenderer.sharedMaterial;

        bool useColorGroups = this.mode == ColorChangeMode.colorgroups;
        this.material.SetInt("_UseColorGroup", Convert.ToInt32(useColorGroups));

        this.material.SetInt("_Swap", Convert.ToInt32(this.swapColors));
        this.material.SetInt("_AddGlow", Convert.ToInt32(this.addGlow));

        this.material.SetInt("_Invert", Convert.ToInt32(this.invert));
        this.material.SetInt("_UseTint", Convert.ToInt32(this.useTint));

        Clear();

        if (useColorGroups) ChangeColorGroups();
        else ChangeColorPalettes();
    }

    public void ChangeColorGroups()
    {
        ChangeColorGroup(0);
        ChangeColorGroup(1);
    }

    private void ChangeColorGroup(int index)
    {
        if (palettes.Count > index && palettes[index].from != null && palettes[index].to != null)
        {
            if (palettes[index].from.colors.Count > 0) this.material.SetColor("_Color_" + ((index * 4) + 1), palettes[index].from.colors[0]);
            if (palettes[index].from.colors.Count > 1) this.material.SetColor("_Color_" + ((index * 4) + 2), palettes[index].from.colors[1]);
            if (palettes[index].from.colors.Count > 2) this.material.SetColor("_Color_" + ((index * 4) + 3), palettes[index].from.colors[2]);
            if (palettes[index].from.colors.Count > 3) this.material.SetColor("_Color_" + ((index * 4) + 4), palettes[index].from.colors[3]);

            if (palettes[index].to.colors.Count > 0) this.material.SetColor("_New_ColorGroup_" + (index + 1), palettes[index].to.colors[0]);
            if (palettes[index].to.addGlow) AddGlow(palettes[index].from.glowColor, palettes[index].to.glow);
        }
    }


    private void AddGlow(Color select, Color glow)
    {
        this.material.SetColor("_Color_Highlight", select);
        this.material.SetColor("_New_Highlight", glow);
    }


    private void Clear()
    {
        for (int i = 1; i <= this.maxAmount; i++)
        {
            this.material.SetColor("_Color_" + i, Color.black);
            this.material.SetColor("_New_Color_" + i, Color.black);
        }

        this.material.SetColor("_New_ColorGroup_1", Color.black);
        this.material.SetColor("_New_ColorGroup_2", Color.black);

        this.material.SetColor("_Color_Highlight", Color.black);
        this.material.SetColor("_New_Highlight", Color.black);
    }


    public void ChangeColorPalettes()
    {
        int count = 1;

        foreach (ColorPalettes palette in this.palettes)
        {
            if (palette.from == null || palette.to == null) break;

            for (int i = 0; i < palette.from.colors.Count; i++)
            {
                Color from = palette.from.colors[i];

                if (count > this.maxAmount || i >= palette.to.colors.Count) break;

                Color to = palette.to.colors[i];

                this.material.SetColor("_Color_" + count, from);
                this.material.SetColor("_New_Color_" + count, to);
                count++;
            }

            if(palette.to.addGlow) AddGlow(palette.from.glowColor, palette.to.glow);

            if (count > this.maxAmount) break;
        }
    }
}
