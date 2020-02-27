using System.Collections.Generic;
using UnityEngine;

public class CharacterPartHandler : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private SpriteRendererExtensionHandler handler;

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

            CharacterPartData data = this.player.preset.GetCharacterPartData(part.property.parentName, part.property.partName);
            Color color = this.player.preset.getColor(part.property.colorGroup);

            if (data != null || part.property.neverDisable) part.gameObject.SetActive(true); 
            if (part.gameObject.activeInHierarchy) part.GetComponent<SpriteRenderer>().color = color;            
        }

        this.handler.init();
    }
}
