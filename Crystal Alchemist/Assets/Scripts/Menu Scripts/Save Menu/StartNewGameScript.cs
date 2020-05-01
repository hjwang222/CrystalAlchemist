using UnityEngine.SceneManagement;
using UnityEngine;

public class StartNewGameScript : MonoBehaviour
{
    [SerializeField]
    private PlayerSaveGame saveGame;

    [SerializeField]
    private string firstScene = "Void";

    public void StartNewGame()
    {
        Cursor.visible = false;
        this.saveGame.Clear();
        SceneManager.LoadSceneAsync(this.firstScene);
    }
}
