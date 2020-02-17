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

            CharacterPartData data = preset.getData(part.gameObject.name, part.transform.parent.name);
            Color color = preset.getColor(part.colorGroup);
            bool isEnabledByRace = part.raceEnabled(preset.race);

            if (part.enableIt(preset.race, data)) part.gameObject.SetActive(true); 
            if (part.gameObject.activeInHierarchy && part.dyeable) part.GetComponent<SpriteRenderer>().color = color;            
        }
    }


}
