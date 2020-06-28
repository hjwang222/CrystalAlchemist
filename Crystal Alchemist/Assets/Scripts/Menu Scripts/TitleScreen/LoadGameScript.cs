using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadGameScript : MonoBehaviour
{
    [SerializeField]
    private PlayerSaveGame saveGame;

    public void LoadGame(SaveSlot slot)
    {
        if (slot != null && slot.data != null)
        {
            Cursor.visible = false;
            LoadSystem.loadPlayerData(this.saveGame, slot.data); //load from data into savegame            
            SceneManager.LoadSceneAsync(this.saveGame.teleportList.nextTeleport.scene);
        }
    }
}
