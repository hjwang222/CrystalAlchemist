using UnityEngine.SceneManagement;

public class PauseMenu : MenuManager
{ 
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
