using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterCreatorPreview : MonoBehaviour
{
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    private void Start()
    {
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        setImage(this.mainMenu.creatorPreset);
    }

    public void setImage(CharacterPreset preset)
    {
        List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorPart>(this.transform, parts);

        foreach (CharacterCreatorPart part in parts)
        {
            if (part != null)
            {
                part.gameObject.SetActive(false); //set inactive

                Image image = part.GetComponent<Image>();
                CharacterPartData data = preset.GetCharacterPartData(part.property.parentName);
                Color color = preset.getColor(part.property.colorGroup);

                if ((data != null
                    && (part.restrictedRaces.Count == 0 || part.restrictedRaces.Contains(preset.getRace())))
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
