using System.Collections.Generic;
using UnityEngine;

public class CharacterPartHandler : MonoBehaviour
{
    private List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();

    public void UpdateCharacterParts(CharacterPreset preset)
    {
        this.parts.Clear();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorPart>(this.transform, this.parts);

        foreach (CharacterCreatorPart part in this.parts)
        {
            part.gameObject.SetActive(false);

            CharacterPartData data = preset.GetCharacterPartData(part.property.parentName, part.property.partName);
            Color color = preset.getColor(part.property.colorGroup);

            if (data != null || part.property.neverDisable) part.gameObject.SetActive(true); 
            if (part.gameObject.activeInHierarchy) part.GetComponent<SpriteRenderer>().color = color;            
        }
    }
}
