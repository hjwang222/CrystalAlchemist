using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Rendering/Base Color Palette")]
public class ColorPaletteBase : ScriptableObject
{
    [InfoBox("In Colorgroup Mode, you can use max 4 colors. Light, Main, Shade, Line")]
    public List<Color> colors = new List<Color>();

    public Color glowColor = Color.white;
}
