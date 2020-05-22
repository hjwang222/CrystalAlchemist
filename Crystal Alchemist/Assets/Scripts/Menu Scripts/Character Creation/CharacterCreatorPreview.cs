using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterCreatorPreview : MonoBehaviour
{
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    public void UpdatePreview()
    {
        List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();
        UnityUtil.GetChildObjects<CharacterCreatorPart>(this.transform, parts);

        foreach (CharacterCreatorPart part in parts)
        {
            if (part != null)
            {
                part.gameObject.SetActive(false); //set inactive

                Image image = part.GetComponent<Image>();
                CharacterPartData data = this.mainMenu.creatorPreset.GetCharacterPartData(part.property.parentName);

                if ((data != null
                    && (part.restrictedRaces.Count == 0 || part.restrictedRaces.Contains(this.mainMenu.creatorPreset.getRace())))
                    || part.property.neverDisable) part.gameObject.SetActive(true); //set active when found (tail?) or always active

                if (part.gameObject.activeInHierarchy && image != null) //set Image and Color when active
                {
                    if (data != null)
                    {
                        CharacterCreatorPartProperty property = this.mainMenu.GetProperty(data.name, data.parentName);
                        if (property != null) part.property = property;

                        Sprite sprite = part.property.GetSprite(part.isFront);

                        if (sprite != null) image.sprite = sprite;
                        else part.gameObject.SetActive(false);
                    }

                    List<Color> colors = this.mainMenu.creatorPreset.getColors(part.property.colorTables);
                    part.SetColors(colors);
                }
            }
        }
    }     


}
