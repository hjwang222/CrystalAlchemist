using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Rendering/New Color Palette")]
public class ColorPaletteNew : ScriptableObject
{
    public List<Color> colors = new List<Color>();

    public bool addGlow = false;

    [ShowIf("addGlow")]
    [ColorUsageAttribute(true, true)]
    public Color glow = Color.white;
}
