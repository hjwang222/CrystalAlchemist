using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour
{
    [Required]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject optionMenu;
    [SerializeField]
    private AudioClip music;
    [SerializeField]
    private TextMeshProUGUI musicUGUI;
    [SerializeField]
    private TextMeshProUGUI effectUGUI;

    [Required]
    [SerializeField]
    private SimpleSignal destroySignal;

    private AudioSource audioSource;
    private AudioSource musicSource;


    void Start()
    {
        showMenu(this.mainMenu);
        SaveSystem.loadOptions();

        setVolumeText(this.musicUGUI, GlobalValues.backgroundMusicVolume);
        setVolumeText(this.effectUGUI, GlobalValues.soundEffectVolume);

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        if (this.music != null)
        {
            this.musicSource = this.transform.gameObject.AddComponent<AudioSource>();
            this.musicSource.clip = this.music;
            this.musicSource.volume = GlobalValues.backgroundMusicVolume;
            this.musicSource.loop = true;
            this.musicSource.Play();
        }

        destroySignal.Raise();
    }

    public void startGame(string scene)
    {
        PlayerData data = SaveSystem.loadPlayer();

        if (data != null && data.scene != null && data.scene != "") scene = data.scene;

        SceneManager.LoadScene(scene);
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

    public void addVolume(TextMeshProUGUI ugui)
    {
        changeVolume(ugui, 1);
    }

    public void reduceVolume(TextMeshProUGUI ugui)
    {
        changeVolume(ugui, -1);
    }

    private void changeVolume(TextMeshProUGUI ugui, int value)
    {
         if (ugui.gameObject.transform.parent.GetComponent<TextMeshProUGUI>().text.Contains("Musik"))
        {
            GlobalValues.backgroundMusicVolume = addVolume(GlobalValues.backgroundMusicVolume, value);
            setVolumeText(ugui, GlobalValues.backgroundMusicVolume);
            this.musicSource.volume = GlobalValues.backgroundMusicVolume;
        }
        else           
        {
            GlobalValues.soundEffectVolume = addVolume(GlobalValues.soundEffectVolume, value);
            setVolumeText(ugui, GlobalValues.soundEffectVolume);
        }        
    }
    
    private void setVolumeText(TextMeshProUGUI ugui, float volume)
    {
        ugui.text = Mathf.RoundToInt(volume * 100) + "%";
    }

    private float addVolume(float volume, float addvolume)
    {
        if (addvolume != 0)
        {
            //if (this.cursorSound != null) Utilities.playSoundEffect(this.audioSource, this.cursorSound);
            volume += (addvolume / 100);
            if (volume < 0) volume = 0;
            else if (volume > 2f) volume = 2f;
        }

        return volume;
    }   
}
