using UnityEngine.SceneManagement;

public class PauseMenu : MenuManager
{
    public override void Start()
    {
        base.Start();
        MenuEvents.current.OnPause += ExitMenu;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MenuEvents.current.OnPause -= ExitMenu;
    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
