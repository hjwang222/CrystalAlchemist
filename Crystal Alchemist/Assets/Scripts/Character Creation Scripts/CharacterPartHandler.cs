using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPartHandler : MonoBehaviour
{
    private List<CharacterPart> parts = new List<CharacterPart>();

    private void GetParts(Transform transform, List<CharacterPart> childObjects)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CharacterPart>() != null) childObjects.Add(child.GetComponent<CharacterPart>());

            GetParts(child, childObjects);
        }
    }

    public void UpdateCharacterParts(CharacterPreset preset)
    {
        GetParts(this.transform, this.parts);

        foreach(CharacterPart part in this.parts)
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
                        if (part.dyeable) part.GetComponent<SpriteRenderer>().color = data.color;
                        break;
                    }
                }
            
        }
    }
}
