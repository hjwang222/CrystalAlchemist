using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveGameScript : MonoBehaviour
{
    [SerializeField]
    private MenuDialogBoxLauncher launcher;

    [SerializeField]
    private PlayerSaveGame saveGame;

    [SerializeField]
    private SaveGameMenu menu;

    public void SaveGame(SaveSlot slot)
    {
        this.saveGame.currentScene = SceneManager.GetActiveScene().name;
        SaveSystem.Save(this.saveGame, slot.gameObject.name); //saves savegame to data
        
        this.menu.UpdateSaves();
        if (this.launcher != null) this.launcher.ShowDialogBox();
    }
}
