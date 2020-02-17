using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CharacterCreatorMenu : MonoBehaviour
{
    [SerializeField]
    private CharacterPreset creatorPreset;

    [SerializeField]
    private CharacterPreset playerPreset;

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
        foreach(Transform child in this.transform)
        {
            CharacterCreatorGear gearButton = child.GetComponent<CharacterCreatorGear>();
            if (gearButton != null)
            {
                CharacterPartData characterPartData = this.creatorPreset.GetCharacterPartData(gearButton.partName, gearButton.parentName);

                if (!gearButton.raceEnabled(this.creatorPreset.race) && (characterPartData != null))
                    this.creatorPreset.RemoveCharacterPartData(gearButton.partName, gearButton.parentName);               
            }
        }
    }


}
