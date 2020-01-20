using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameMenu : MenuControls
{
    public void saveGame(GameObject slot)
    {
        Scene scene = SceneManager.GetActiveScene();
        SaveSystem.Save(this.player, scene.name, slot.name);
        this.exitMenu();
    }
}


