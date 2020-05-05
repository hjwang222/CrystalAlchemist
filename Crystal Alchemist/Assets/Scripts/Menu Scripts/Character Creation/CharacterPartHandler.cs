using System.Collections.Generic;
using UnityEngine;

public class CharacterPartHandler : MonoBehaviour
{
    [SerializeField]
    private CharacterPreset preset;

    [SerializeField]
    private SpriteRendererExtensionHandler handler;

    private List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();

    public void UpdateCharacterParts()
    {
        this.parts.Clear();
        UnityUtil.GetChildObjects<CharacterCreatorPart>(this.transform, this.parts);

        foreach (CharacterCreatorPart part in this.parts)
        {
            part.gameObject.SetActive(false);

            CharacterPartData data = this.preset.GetCharacterPartData(part.property.parentName, part.property.partName);
            Color color = this.preset.getColor(part.property.colorGroup);

            if (data != null || part.property.neverDisable) part.gameObject.SetActive(true); 
            if (part.gameObject.activeInHierarchy) part.GetComponent<SpriteRenderer>().color = color;            
        }

        this.handler.Initialize();
    }
}
