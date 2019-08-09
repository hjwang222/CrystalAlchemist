using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    private SimpleSignal stopMusic;

    [SerializeField]
    private BoolValue loadGame;

    [SerializeField]
    private AudioSource audiosource;

    [SerializeField]
    private AudioClip deathMusic;

    [SerializeField]
    private PostProcessVolume colorGrading;

    [SerializeField]
    private TextMeshProUGUI textField;

    [SerializeField]
    private TextMeshProUGUI countDown;

    [SerializeField]
    private GameObject returnTitleScreen;

    [SerializeField]
    private GameObject returnSavePoint;

    [SerializeField]
    private int timer = 30;

    private string currentText;
    private string fullText;
    private string lastSavepoint;

    private void Start()
    {
        this.returnSavePoint.SetActive(false);
        this.returnTitleScreen.SetActive(false);
        this.countDown.gameObject.SetActive(false);
        this.textField.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        PlayerData data = SaveSystem.loadPlayer();
        if (data != null && data.scene != null && data.scene != "") this.lastSavepoint = data.scene;
        this.stopMusic.Raise();

        //this.colorGrading.GetComponent<ColorGrading>().active = true;
        showText();

        //if(this.colorGrading.GetComponent<ColorGrading>() != null) StartCoroutine(FadeOut(0.1f));
    }

    private void showText()
    {
        playMusic(this.deathMusic);
        this.textField.gameObject.SetActive(true);
        ShowText(0.1f);
    }

    public void playMusic(AudioClip clip)
    {
        Utilities.Audio.playSoundEffect(this.audiosource, clip, GlobalValues.backgroundMusicVolume);
    }

    public void ShowText(float delay)
    {
        this.fullText = this.textField.text;
        StartCoroutine(this.ShowTextCo(delay));
    }

    public void showButtons()
    {
        this.returnTitleScreen.SetActive(true);
        if (this.lastSavepoint != null) this.returnSavePoint.SetActive(true);

        this.countDown.gameObject.SetActive(true);
        StartCoroutine(this.countDownCo());
    }

    public void returnToTitleScreen()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void returnSaveGame()
    {
        this.loadGame.setValue(true);
        if (this.lastSavepoint != null) SceneManager.LoadSceneAsync(this.lastSavepoint);
    }

    private IEnumerator ShowTextCo(float delay)
    {
        for(int i = 0; i < this.fullText.Length; i++)
        {
            if(i >= this.fullText.Length)
            {
                showButtons();
                break;
            }

            currentText = this.fullText.Substring(0, i);
            this.textField.text = this.currentText;
            Debug.Log(this.currentText);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator countDownCo()
    {
        for (int i = 0; i < this.timer; i++)
        {
            if (i >= this.timer)
            {
                returnToTitleScreen();
                break;
            }
            this.countDown.text = "" + i + "s";
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator FadeOut(float delay)
    {
        while (this.colorGrading.GetComponent<ColorGrading>().saturation.value > -100)
        {
            if (this.colorGrading.GetComponent<ColorGrading>().saturation.value <= -100)
            {
                showText();
                break;
            }

            this.colorGrading.GetComponent<ColorGrading>().saturation.value -= 1f;
            yield return new WaitForSeconds(delay);
        }
    }
}
