using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "CharacterCreation/Color Palette")]
public class CharacterCreatorColorPalette : ScriptableObject
{
    public List<Color> colors = new List<Color>() { Color.white };

    [Button]
    public void setAlpha()
    {
        for(int i = 0; i < this.colors.Count; i++)
        {
            this.colors[i] = new Color(this.colors[i].r, this.colors[i].g, this.colors[i].b, 1);
        }
    }
}
