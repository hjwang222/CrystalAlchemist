using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/CharacterCreation/Color Palette")]
public class CharacterCreatorColorPalette : ScriptableObject
{
    public List<Color> colors = new List<Color>();
}
