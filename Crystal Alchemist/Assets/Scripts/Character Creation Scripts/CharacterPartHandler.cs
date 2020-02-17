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

            CharacterPartData data = preset.GetCharacterPartData(part.partName, part.parentName);
            Color color = preset.getColor(part.colorGroup);

            if (data != null || part.neverDisable) part.gameObject.SetActive(true); 
            if (part.gameObject.activeInHierarchy) part.GetComponent<SpriteRenderer>().color = color;            
        }
    }
}
