using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class CharacterCreatorPreview : MonoBehaviour
{
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    [SerializeField]
    private TextMeshProUGUI previewText;

    public void UpdatePreview()
    {
        this.previewText.text = this.mainMenu.creatorPreset.characterName;

        List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorPart>(this.transform, parts);

        foreach (CharacterCreatorPart part in parts)
        {
            if (part != null)
            {
                part.gameObject.SetActive(false); //set inactive

                Image image = part.GetComponent<Image>();
                CharacterPartData data = this.mainMenu.creatorPreset.GetCharacterPartData(part.property.parentName);
                Color color = this.mainMenu.creatorPreset.getColor(part.property.colorGroup);

                if ((data != null
                    && (part.restrictedRaces.Count == 0 || part.restrictedRaces.Contains(this.mainMenu.creatorPreset.getRace())))
                    || part.property.neverDisable) part.gameObject.SetActive(true); //set active when found (tail?) or always active

                if (part.gameObject.activeInHierarchy && image != null) //set Image and Color when active
                {
                    image.color = color;

                    if (data != null)
                    {
                        Sprite sprite = getSprite(data, part.property.category, part.previewDirection);
                        if (sprite != null) image.sprite = sprite;
                        else part.gameObject.SetActive(false);
                    }
                }
            }

        }
    }

    private Sprite getSprite(CharacterPartData data, string directory, string direction)
    {
        string path = "Art/Characters/Player Sprites/" + directory + "/" + data.parentName + "/" + data.name;
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        foreach (Sprite sprite in sprites)
        {
            if (sprite.name.Contains(("Idle " + direction))) return sprite;
        }

        return null;
    }
}
