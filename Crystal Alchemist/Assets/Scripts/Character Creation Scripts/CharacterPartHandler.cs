using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPartHandler : MonoBehaviour
{
    private List<CharacterPart> parts = new List<CharacterPart>();

    public void UpdateCharacterParts(CharacterPreset preset)
    {
        this.parts.Clear();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterPart>(this.transform, this.parts);

        foreach (CharacterPart part in this.parts)
        {
            part.gameObject.SetActive(false);

            foreach (CharacterPartData data in preset.datas)
            {
                if ((data.type == part.colorGroup)
                    && (part.restrictedRaces.Count == 0 || part.restrictedRaces.Contains(preset.race))
                    && (!part.useName || (data.name.ToUpper() == part.gameObject.name.ToUpper()))
                    && (!part.isEarHorn || (part.isEarHorn && preset.addEarsHorns)))
                {
                    part.gameObject.SetActive(true);

                    Color color = Color.white;
                    if (part.dyeable) color = data.color;
                    part.GetComponent<SpriteRenderer>().color = color;

                    savePreview(preset, part, color);                     
                    break;
                }
            }
        }
    }

    private void savePreview(CharacterPreset preset, CharacterPart part, Color color)
    {
        Sprite front = null;
        Sprite back = null;

        if (part.savePreview)
        {
            string path = "Art/Characters/Player Sprites/" + part.assetPath.Replace(".png", "");
            Sprite[] sprites = Resources.LoadAll<Sprite>(path);

            foreach (Sprite sprite in sprites)
            {
                if (sprite.name.Contains("Idle Down")) front = sprite;
                if (sprite.name.Contains("Idle Up")) back = sprite;
            }
        }

        preset.setPreview(part.transform.parent.name, front, back, color);
    }

    
}
