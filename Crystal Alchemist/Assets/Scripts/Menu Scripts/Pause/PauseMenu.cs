using UnityEngine.SceneManagement;

public class PauseMenu : MenuBehaviour
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

    public void ExitGame() => SceneManager.LoadSceneAsync(0);    

    public void SaveSettings() => SaveSystem.SaveOptions();    
}
