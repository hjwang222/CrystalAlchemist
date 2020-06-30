using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorColor : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private CharacterCreatorColorPaletteHandler handler;

    public void Click()
    {
        this.handler.mainMenu.creatorPreset.AddColorGroup(this.handler.colorGroup, this.image.color);
        handler.Click();
    }

    public ColorGroup GetColorGroup()
    {
        return this.handler.colorGroup;
    }
}
