using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [BoxGroup("Menu")]
    [SerializeField]
    private string sceneName;

    [BoxGroup("Menu")]
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [Required]
    [BoxGroup("Menu")]
    public CharacterValues playerValues;

    [BoxGroup("Menu")]
    public InfoBox infoBox;

    private GameObject sel;

    public virtual void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject != sel)
            sel = EventSystem.current.currentSelectedGameObject;
        else if (sel != null && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(sel);
    }

    public virtual void Start()
    {
        IngameMenuHandler.openedMenues.Add(this.gameObject);

        if (IngameMenuHandler.openedMenues.Count <= 1)
        {
            Cursor.visible = true;

            IngameMenuHandler.lastState = this.playerValues.currentState;
            GameEvents.current.DoMenuOpen(CharacterState.inMenu);
            GameEvents.current.DoMenuOverlay(true);

            if (this.musicVolumeSignal != null) this.musicVolumeSignal.Raise(MasterManager.settings.getMusicInMenu());
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

            if (this.musicVolumeSignal != null) this.musicVolumeSignal.Raise(MasterManager.settings.backgroundMusicVolume);
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
