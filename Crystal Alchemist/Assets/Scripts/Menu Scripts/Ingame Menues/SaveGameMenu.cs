using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGameMenu : MenuControls
{
    [SerializeField]
    private GameObject success;

    [SerializeField]
    private List<TitleScreenSaveSlot> slots = new List<TitleScreenSaveSlot>();

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    [SerializeField]
    private AudioClip soundeffect;

    public override void OnEnable()
    {
        base.OnEnable();
        this.success.SetActive(false);
    }

    public void saveGame(GameObject slot)
    {
        Scene scene = SceneManager.GetActiveScene();
        SaveSystem.Save(this.player, scene.name, slot.name);
        CustomUtilities.Audio.playSoundEffect(this.soundeffect, GlobalValues.backgroundMusicVolume);

        enableButtons(false);
        this.success.SetActive(true);
    }

    private void enableButtons(bool value)
    {
        foreach (Button button in this.buttons)
        {
            button.enabled = value;
            button.GetComponent<ButtonExtension>().enabled = value;
            if(value) button.GetComponent<ButtonExtension>().setFirst();
        }
    }

    public void updateSaves()
    {
        foreach(TitleScreenSaveSlot slot in this.slots)
        {
            slot.getData();
        }

        enableButtons(true);

        this.success.SetActive(false);
    }
}


