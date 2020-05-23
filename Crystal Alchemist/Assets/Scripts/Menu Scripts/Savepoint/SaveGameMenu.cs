using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SaveGameMenu : MonoBehaviour
{
    [BoxGroup("Save Menu")]
    [Required]
    [SerializeField]
    private List<SaveSlot> slots = new List<SaveSlot>();

    [BoxGroup("Save Menu")]
    [Required]
    [SerializeField]
    private TeleportStats savePointInfo;

    [BoxGroup("Save Menu")]
    [SerializeField]
    private MenuDialogBoxLauncher launcher;

    [BoxGroup("Save Menu")]
    [SerializeField]
    private PlayerSaveGame saveGame;

    //Called from Dialogbox
    public void UpdateSaves()
    {
        foreach (SaveSlot slot in this.slots)
        {
            slot.getData();
            slot.GetComponent<ButtonExtension>().SetFirst();
        }
    }

    public void SaveGame(SaveSlot slot)
    {
        this.saveGame.startSpawnPoint.SetValue(this.savePointInfo); //set as new Spawnpoint
        SaveSystem.Save(this.saveGame, slot.gameObject.name); //saves savegame to data

        UpdateSaves();
        if (this.launcher != null) this.launcher.ShowDialogBox();
    }
}


