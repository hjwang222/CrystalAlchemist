using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorColorPaletteHandler : CharacterCreatorButton
{
    [SerializeField]
    private CharacterCreatorColorPalette palette;

    [SerializeField]
    private GameObject colorPick;

    [SerializeField]
    private bool setFirst = false;

   public ColorGroup colorGroup;

    private void Start()
    {
        colorPick.SetActive(false);

        for(int i = 0; i < this.palette.colors.Count; i++)
        {
            Color color = this.palette.colors[i];

            GameObject newColorPicker = Instantiate(this.colorPick, this.transform);            

            if (i == 0 && this.setFirst) newColorPicker.GetComponent<ButtonExtension>().SetAsFirst();

            newColorPicker.SetActive(true);

            if (newColorPicker.transform.childCount > 0 && newColorPicker.transform.GetChild(0).GetComponent<Image>() != null)
                newColorPicker.transform.GetChild(0).GetComponent<Image>().color = color;

        }
    }
}
