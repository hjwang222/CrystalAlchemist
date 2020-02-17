using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterCreatorPreview : MonoBehaviour
{
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterPreset preset;

    [BoxGroup("Required")]
    [SerializeField]
    private List<GameObject> previews = new List<GameObject>();

    private void Start()
    {
        UpdatePreview(this.preset);
    }

    private void UpdatePreview(CharacterPreset preset)
    {
        //setImage(preset);
    }

    private void savePreview(CharacterPartData data, CharacterPart part)
    {
        Sprite front = null;
        Sprite back = null;

        string path = "Art/Characters/Player Sprites/" + part.assetPath.Replace(".png", "");
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        foreach (Sprite sprite in sprites)
        {
            if (sprite.name.Contains("Idle Down")) front = sprite;
            if (sprite.name.Contains("Idle Up")) back = sprite;
        }
        data.parentName = part.transform.parent.name;
        data.setSprites(front, back);
    }

    public void setImage(CharacterPreset preset)
    {
        for (int i = 0; i < previews.Count; i++)
        {
            GameObject preview = previews[i];

            foreach (Transform trans in preview.transform)
            {
                GameObject imageGO = trans.gameObject;    
                CharacterCreatorPart part = imageGO.GetComponent<CharacterCreatorPart>();

                if (part != null)
                {
                    imageGO.SetActive(false);
                    Image image = part.GetComponent<Image>();

                    CharacterPartData data = preset.getData(null, imageGO.name);
                    Color color = preset.getColor(part.colorGroup);

                    if (data != null || part.neverDisable) imageGO.SetActive(true);

                    if (imageGO.activeInHierarchy)
                    {                        
                        image.color = color;

                        if (data != null)
                        {
                            List<Sprite> sprites = data.previewImages;
                            if (sprites.Count > 0) image.sprite = sprites[i];
                        }
                    }
                }
            }
        }
    }
}
