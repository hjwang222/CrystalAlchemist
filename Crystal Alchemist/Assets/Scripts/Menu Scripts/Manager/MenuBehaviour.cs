using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MenuBehaviour : PreventDeselection
{
    [BoxGroup("Menu")]
    [SerializeField]
    private string sceneName;

    [BoxGroup("Menu")]
    [SerializeField]
    private bool changeVolume;

    [Required]
    [BoxGroup("Menu")]
    public CharacterValues playerValues;

    [BoxGroup("Menu")]
    [SerializeField]
    private bool showBlackBackground = true;

    [BoxGroup("Menu")]
    public InfoBox infoBox;

    public virtual void Start()
    {
        IngameMenuHandler.openedMenues.Add(this.gameObject);

        if (IngameMenuHandler.openedMenues.Count <= 1)
        {
            Cursor.visible = true;

            IngameMenuHandler.lastState = this.playerValues.currentState;
            GameEvents.current.DoMenuOpen(CharacterState.inMenu);
            if(this.showBlackBackground) GameEvents.current.DoMenuOverlay(true);

            if(this.changeVolume) MusicEvents.current.ChangeVolume(MasterManager.settings.GetMenuVolume());
        }
    }

    public virtual void OnDestroy()
    {
        IngameMenuHandler.openedMenues.Remove(this.gameObject);

        if (IngameMenuHandler.openedMenues.Count <= 0)
        {
            Cursor.visible = false;
            GameEvents.current.DoMenuClose(IngameMenuHandler.lastState);//avoid reclick!
            GameEvents.current.DoMenuOverlay(false);

            if (this.changeVolume) MusicEvents.current.ChangeVolume(MasterManager.settings.backgroundMusicVolume);
        }
    }

    public virtual void Cancel()
    {
        if (this.infoBox != null && this.infoBox.gameObject.activeInHierarchy) this.infoBox.Hide();
        else ExitMenu();
    }

    public virtual void ExitMenu()
    {
        SceneManager.UnloadSceneAsync(this.sceneName);
    }
}
