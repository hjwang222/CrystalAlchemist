using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ColorPalette : ScriptableObject
{    
    public List<Color> colors = new List<Color>();
}

[CreateAssetMenu(menuName = "Game/Color Palette/Base")]
public class ColorPaletteBase : ColorPalette
{
    public Color selectHighlight = Color.white;
}

[CreateAssetMenu(menuName = "Game/Color Palette/Override")]
public class ColorPaletteOverride : ColorPalette
{
    public bool addGlow = false;

    [ShowIf("addGlow")]
    [ColorUsageAttribute(true, true)]
    public Color highlight = Color.white;
}