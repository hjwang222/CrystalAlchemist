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
                    part.gameObject.SetActive(false);
                    Image image = part.GetComponent<Image>();

                    CharacterPartData data = preset.GetCharacterPartData(part.partGroup);
                    Color color = preset.getColor(part.colorGroup);

                    if (data != null || part.neverDisable) part.gameObject.SetActive(true);

                    if (part.gameObject.activeInHierarchy && image != null)
                    {                        
                        image.color = color;

                        if (data != null)
                        {
                            Sprite sprite = getSprite(data, part.directory, part.direction);
                            if (sprite != null) image.sprite = sprite;
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
