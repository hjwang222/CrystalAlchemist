using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    [BoxGroup("Mandatory")]
    private SimpleSignal stopMusic;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private BoolValue loadGame;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private AudioSource audiosource;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private AudioClip deathMusic;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private PostProcessVolume postProcessVolume;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private myCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private TextMeshProUGUI textField;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private TextMeshProUGUI countDown;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject UI;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject returnTitleScreen;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private GameObject returnSavePoint;

    [BoxGroup("Time")]
    [SerializeField]
    private int timer = 30;

    [BoxGroup("Time")]
    [SerializeField]
    private float fadingDelay = 0.025f;

    [BoxGroup("Time")]
    [SerializeField]
    private float textDelay = 0.1f;

    [BoxGroup("Time")]
    [SerializeField]
    private float buttonDelay = 2f;

    private string currentText;
    private string fullText;
    private string lastSavepoint;
    private ColorGrading colorGrading;
    private Player player;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        this.returnSavePoint.SetActive(false);
        this.returnTitleScreen.SetActive(false);
        this.countDown.gameObject.SetActive(false);
        this.textField.gameObject.SetActive(false);
        this.UI.SetActive(false);
    }


    private void OnEnable()
    {
        PlayerData data = SaveSystem.loadPlayer();
        if (data != null && data.scene != null && data.scene != "") this.lastSavepoint = data.scene;
        this.stopMusic.Raise();
        if (this.postProcessVolume.profile.TryGetSettings(out this.colorGrading)) StartCoroutine(FadeOut(this.fadingDelay));
    }

    private void showText()
    {
        playMusic(this.deathMusic);
        this.textField.gameObject.SetActive(true);
        ShowText(this.textDelay);
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
        this.cursor.gameObject.SetActive(true);
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
        if (this.lastSavepoint != null)
        {
            this.colorGrading.saturation.value = 0;
            this.colorGrading.colorFilter.value = Color.white;

            this.gameObject.SetActive(false);
            this.UI.SetActive(true);
            SceneManager.LoadSceneAsync(this.lastSavepoint);
            this.player.initPlayer();
        }
    }

    private IEnumerator ShowTextCo(float delay)
    {
        for(int i = 0; i <= this.fullText.Length; i++)
        {
            currentText = this.fullText.Substring(0, i);
            this.textField.text = this.currentText;

            if (i >= this.fullText.Length)
            {
                StartCoroutine(showButtonCo(this.buttonDelay));
                break;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator showButtonCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        showButtons();
    }

    private IEnumerator countDownCo()
    {
        for (int i = 0; i < this.timer; i++)
        {
            if (i >= this.timer)
            {
                returnToTitleScreen();
            }
            this.countDown.text = "" + (this.timer-i) + "s";
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator FadeOut(float delay)
    {
        while (this.colorGrading.saturation.value > -100)
        {
            this.colorGrading.saturation.value -= 1f;

           /* this.colorGrading.colorFilter.value.g -= 2.4f;
            this.colorGrading.colorFilter.value.b -= 2.4f;
            this.colorGrading.colorFilter.value.r -= 2.4f;*/

            if (this.colorGrading.saturation.value <= -100)
            {
                showText();
                break;
            }
            
            yield return new WaitForSeconds(delay);
        }
    }
}
