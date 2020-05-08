using UnityEngine;

public class SaveGameScript : MonoBehaviour
{
    [SerializeField]
    private MenuDialogBoxLauncher launcher;

    [SerializeField]
    private PlayerSaveGame saveGame;

    [SerializeField]
    private SimpleSignal updateSaveSignal;

    public void SaveGame(SaveSlot slot)
    {        
        SaveSystem.Save(this.saveGame, slot.gameObject.name); //saves savegame to data
        
        this.updateSaveSignal.Raise();
        if (this.launcher != null) this.launcher.ShowDialogBox();
    }
}
