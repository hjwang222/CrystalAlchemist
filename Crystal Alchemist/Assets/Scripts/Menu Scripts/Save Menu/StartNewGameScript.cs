using UnityEngine.SceneManagement;
using UnityEngine;

public class StartNewGameScript : MonoBehaviour
{
    [SerializeField]
    private PlayerSaveGame saveGame;

    [SerializeField]
    private string firstScene = "Void";

    [SerializeField]
    private Vector2 position = Vector2.zero;

    [SerializeField]
    private TeleportStats nextTeleport;

    public void StartNewGame()
    {
        Cursor.visible = false;
        this.saveGame.Clear();
        this.nextTeleport.SetValue(this.firstScene, this.position);

        SceneManager.LoadSceneAsync(this.nextTeleport.scene);
    }
}
