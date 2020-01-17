
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> menues = new List<GameObject>(); 
    [SerializeField]
    private AudioClip music;

    [SerializeField]
    private GameObject mainFrame;
    [SerializeField]
    private GameObject darkFrame;

    [SerializeField]
    private Image continueUGUI;
    [SerializeField]
    private Color disabledColor;
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
        showMenu(this.menues[0]);
        this.darkFrame.SetActive(false);

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

        if (this.lastSavepoint == null) this.continueUGUI.color = this.disabledColor;
        else this.continueUGUI.color = Color.white;

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
        this.continueUGUI.color = this.disabledColor;
    }

    public void continueGame()
    {
        this.loadGame.setValue(true);
        if(this.lastSavepoint != null) SceneManager.LoadSceneAsync(this.lastSavepoint);
        Cursor.visible = false;
    }

    public void exitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void save()
    {
        SaveSystem.SaveOptions();        
    }

    public void showMenu(GameObject newActiveMenu)
    {
        foreach(GameObject gameObject in this.menues)
        {
            gameObject.SetActive(false);
        }

        newActiveMenu.SetActive(true);
        
        for(int i = 0; i < newActiveMenu.transform.childCount; i++)
        {
            ButtonExtension temp = newActiveMenu.transform.GetChild(i).GetComponent<ButtonExtension>();
            if(temp != null && temp.setFirstSelected)
            {
                temp.setFirst();
                break;
            }
        }
    }

    public void showBackground(bool dark)
    {
        this.mainFrame.SetActive(false);
        this.darkFrame.SetActive(false);

        if (dark) this.darkFrame.SetActive(true);
        else this.mainFrame.SetActive(true);
    }

   
}
