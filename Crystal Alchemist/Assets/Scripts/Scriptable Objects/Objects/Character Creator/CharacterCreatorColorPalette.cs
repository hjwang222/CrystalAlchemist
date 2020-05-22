using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ColorInfo
{
    public Color color = Color.white;

    public bool addGlow = false;

    [ShowIf("addGlow")]
    [ColorUsageAttribute(true, true)]
    public Color glow = Color.white;

    public ColorInfo(Color color)
    {
        this.color = color;
    }
}


[CreateAssetMenu(menuName = "Game/CharacterCreation/Color Palette")]
public class CharacterCreatorColorPalette : ScriptableObject
{
    public List<Color> colors = new List<Color>();
}
