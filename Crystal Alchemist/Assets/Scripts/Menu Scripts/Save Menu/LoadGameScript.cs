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
            LoadSystem.loadPlayerData(this.saveGame, slot.gameObject.name); //load from data into savegame            
            SceneManager.LoadSceneAsync(slot.data.scene);
        }
    }
}
