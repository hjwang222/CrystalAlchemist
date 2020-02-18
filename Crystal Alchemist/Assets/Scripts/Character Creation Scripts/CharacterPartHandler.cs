using System.Collections.Generic;
using UnityEngine;

public class CharacterPartHandler : MonoBehaviour
{
    [SerializeField]
    private CharacterPreset preset;

    private List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();

    private void Start()
    {
        UpdateCharacterParts();
    }

    public void UpdateCharacterParts()
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
