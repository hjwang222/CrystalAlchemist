using UnityEngine.SceneManagement;
using UnityEngine;

public class StartNewGameScript : MonoBehaviour
{
    [SerializeField]
    private PlayerSaveGame saveGame;

    [SerializeField]
    private TeleportStats startTeleport;

    [SerializeField]
    private TimeValue timeValue;

    public void StartNewGame()
    {
        Cursor.visible = false;
        this.timeValue.Clear();
        this.saveGame.Clear();
        this.saveGame.teleportList.SetNextTeleport(this.startTeleport);

        SceneManager.LoadSceneAsync(this.saveGame.teleportList.nextTeleport.scene);
    }
}
