using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject optionMenu;
    [SerializeField]
    private AudioClip music;

    [SerializeField]
    private TextMeshProUGUI continueUGUI;
    [SerializeField]
    private Color color;
    [SerializeField]
    private BoolValue loadGame;

    [Required]
    [SerializeField]
    private SimpleSignal destroySignal;

    private AudioSource musicSource;
    private string lastSavepoint = null;

    void Start()
    {
        SaveSystem.loadOptions();
        Cursor.visible = true;
        showMenu(this.mainMenu);

        if (this.music != null)
        {
            this.musicSource = this.transform.gameObject.GetComponent<AudioSource>();
            this.musicSource.clip = this.music;
            this.musicSource.volume = GlobalValues.backgroundMusicVolume;
            this.musicSource.loop = true;
            this.musicSource.Play();
        }

        PlayerData data = SaveSystem.loadPlayer();
        if (data != null && data.scene != null && data.scene != "") this.lastSavepoint = data.scene;

        if (this.lastSavepoint == null) this.continueUGUI.color = Color.gray;
        else this.continueUGUI.color = this.color;

        destroySignal.Raise();
    }

    private void LateUpdate()
    {
        if (!Cursor.visible) Cursor.visible = true;
    }

    public void startGame(string scene)
    {
        this.loadGame.setValue(false);
        SceneManager.LoadSceneAsync(scene);
        Cursor.visible = false;
    }

    public void deleteSaveGame()
    {
        SaveSystem.DeleteSave();
        this.lastSavepoint = null;
        this.continueUGUI.color = Color.gray;
    }

    public void continueGame()
    {
        this.loadGame.setValue(true);
        if(this.lastSavepoint != null) SceneManager.LoadSceneAsync(this.lastSavepoint);
        Cursor.visible = false;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void save()
    {
        SaveSystem.SaveOptions();        
    }

    public void showMenu(GameObject newActiveMenu)
    {
        this.mainMenu.SetActive(false);
        this.optionMenu.SetActive(false);

        newActiveMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(newActiveMenu.transform.GetChild(0).gameObject);
    }

   
}
