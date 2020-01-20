using UnityEngine.SceneManagement;

public class PauseMenu : MenuControls
{ 
    public void exitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
