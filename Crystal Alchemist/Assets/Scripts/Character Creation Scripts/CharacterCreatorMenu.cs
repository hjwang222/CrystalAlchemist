using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CharacterCreatorMenu : BasicMenu
{
    public CharacterPreset creatorPreset;

    [SerializeField]
    private CharacterPreset playerPreset;

    public CharacterPresetSignal presetSignal;

    [SerializeField]
    private List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();

    [Required]
    [SerializeField]
    private string firstScene = "Haus";

    public override void Start()
    {
        base.Start();
        updateGear();
    }

    public void Confirm()
    {
        this.playerPreset = this.creatorPreset; //save Preset
        startTheGame(this.firstScene); //TitleScreen only
    }

    public void startTheGame(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
        Cursor.visible = false;
    }

    public void updateGear()
    {
        List<CharacterCreatorGear> gearButtons = new List<CharacterCreatorGear>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorGear>(this.transform, gearButtons);

        foreach (CharacterCreatorPartProperty part in this.properties)
        {
            enableGearButton(gearButtons, part);
            CharacterPartData data = this.creatorPreset.GetCharacterPartData(part.parentName, part.partName);
            bool enableIt = part.enableIt(this.creatorPreset.race, data);

            if (enableIt) this.creatorPreset.AddCharacterPartData(part.parentName, part.partName);
            else this.creatorPreset.RemoveCharacterPartData(part.parentName, part.partName);
        }

        this.presetSignal.Raise(this.creatorPreset); //Update Preview
    }

    public void updateColor(CharacterCreatorPartProperty property)
    {
        List<CharacterCreatorColor> colorButtons = new List<CharacterCreatorColor>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorColor>(this.transform, colorButtons);

        foreach (CharacterCreatorColor button in colorButtons)
        {
            if (button.colorGroup == property.colorGroup)
            {
                button.gameObject.SetActive(property.isDyeable);
                if (!property.isDyeable) this.creatorPreset.RemoveColorGroup(property.colorGroup);
            }
        }

        this.presetSignal.Raise(this.creatorPreset); //Update Preview
    }

    private void enableGearButton(List<CharacterCreatorGear> gearButtons, CharacterCreatorPartProperty part)
    {
        foreach (CharacterCreatorGear button in gearButtons)
        {
            if (button.property == part)
                button.gameObject.SetActive(part.raceEnabled(this.creatorPreset.race));
        }
    }
}
