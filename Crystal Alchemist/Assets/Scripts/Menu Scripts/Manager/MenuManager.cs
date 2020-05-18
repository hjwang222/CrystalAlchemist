using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        MenuEvents.current.OnInventory += OpenInventory;
        MenuEvents.current.OnPause += OpenPause;
        MenuEvents.current.OnMap += OpenMap;
        MenuEvents.current.OnSkills += OpenSkillBook;
        MenuEvents.current.OnAttributes += OpenAttributes;
        MenuEvents.current.OnDeath += OpenDeath;
        MenuEvents.current.OnMiniGame += OpenMiniGame;
        MenuEvents.current.OnEditor += OpenCharacterEditor;
        MenuEvents.current.OnSave += OpenSavePoint;
        MenuEvents.current.OnDialogBox += OpenDialogBox;
        MenuEvents.current.OnMenuDialogBox += OpenMenuDialogBox;
    }

    private void OnDestroy()
    {
        MenuEvents.current.OnInventory -= OpenInventory;
        MenuEvents.current.OnPause -= OpenPause;
        MenuEvents.current.OnMap -= OpenMap;
        MenuEvents.current.OnSkills -= OpenSkillBook;
        MenuEvents.current.OnAttributes -= OpenAttributes;
        MenuEvents.current.OnDeath -= OpenDeath;
        MenuEvents.current.OnMiniGame -= OpenMiniGame;
        MenuEvents.current.OnEditor -= OpenCharacterEditor;
        MenuEvents.current.OnSave -= OpenSavePoint;
        MenuEvents.current.OnDialogBox -= OpenDialogBox;
        MenuEvents.current.OnMenuDialogBox -= OpenMenuDialogBox;
    }

    public void OpenInventory() => OpenScene("InventoryMenu");

    public void OpenPause() => OpenScene("Pause");

    public void OpenMap() => OpenScene("Map");

    public void OpenSkillBook() => OpenScene("Skillbook");

    public void OpenAttributes() => OpenScene("Attributes");

    public void OpenCharacterEditor() => OpenScene("Character Creation");

    public void OpenSavePoint() => OpenScene("Savepoint");

    public void OpenDeath() => OpenScene("Death Screen");

    public void OpenMiniGame(MiniGame miniGame) => OpenScene("Minigame");

    public void OpenDialogBox() => OpenScene("DialogBox");

    public void OpenMenuDialogBox() => OpenScene("MenuDialogBox");

    private void OpenScene(string scene)
    {
        if (UnityUtil.SceneExists(scene)) SceneManager.UnloadSceneAsync(scene);
        else SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }
}
