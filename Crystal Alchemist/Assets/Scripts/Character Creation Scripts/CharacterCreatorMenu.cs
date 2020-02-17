using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CharacterCreatorMenu : MonoBehaviour
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
        foreach (CharacterCreatorPartProperty part in this.properties)
        {
            CharacterPartData data = this.creatorPreset.GetCharacterPartData(part.parentName, part.name);
            bool enableIt = part.enableIt(this.creatorPreset.race, data);

            if (enableIt)
                this.creatorPreset.AddCharacterPartData(part.parentName, part.partName); //Pony Ohren werden überschrieben da gleicher Parent
            else
                this.creatorPreset.RemoveCharacterPartData(part.parentName, part.partName);
        }

        this.presetSignal.Raise(this.creatorPreset); //Update Preview
    }
}
