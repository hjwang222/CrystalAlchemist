using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/CharacterCreation/Color Palette")]
public class CharacterCreatorColorPalette : ScriptableObject
{
    public List<Color> colors = new List<Color>();
}
