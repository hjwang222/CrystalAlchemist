using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterCreatorPreview : MonoBehaviour
{
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterPreset creatorPreset;

    [BoxGroup("Required")]
    [SerializeField]
    private List<GameObject> previews = new List<GameObject>();

    private void Start()
    {
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        setImage(this.creatorPreset);
    }

    public void setImage(CharacterPreset preset)
    {
        foreach(GameObject preview in previews)
        {       
            foreach (Transform child in preview.transform)
            {
                CharacterCreatorPart part = child.GetComponent<CharacterCreatorPart>();

                if (part != null)
                {
                    part.gameObject.SetActive(false); //set inactive

                    Image image = part.GetComponent<Image>();
                    CharacterPartData data = preset.GetCharacterPartData(part.property.parentName);
                    Color color = preset.getColor(part.property.colorGroup);

                    if (data != null || part.property.neverDisable) part.gameObject.SetActive(true); //set active when found (tail?) or always active

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
