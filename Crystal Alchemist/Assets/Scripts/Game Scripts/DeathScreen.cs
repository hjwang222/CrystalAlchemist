using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

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
    private Volume volume;

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
    private ColorAdjustments colorGrading;
    private Player player;

    private void Awake()
    {
        this.player = this.playerStats.player;
        init();
    }

    private void init()
    {
        this.cursor.gameObject.SetActive(false);
        this.returnSavePoint.SetActive(false);
        this.returnTitleScreen.SetActive(false);
        this.countDown.gameObject.SetActive(false);
        this.textField.gameObject.SetActive(false);
        this.UI.SetActive(false);
    }


    private void OnEnable()
    {
        init();
        this.stopMusic.Raise();
        if (this.volume.profile.TryGet(out this.colorGrading)) StartCoroutine(FadeOut(this.fadingDelay));
    }

    private void showText()
    {
        playMusic(this.deathMusic);
        this.textField.gameObject.SetActive(true);
        ShowText(this.textDelay);
    }

    public void playMusic(AudioClip clip)
    {
        CustomUtilities.Audio.playSoundEffect(this.audiosource, clip, GlobalValues.backgroundMusicVolume);
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
        if (this.player.GetComponent<PlayerTeleport>().getLastTeleport()) this.returnSavePoint.SetActive(true);

        this.countDown.gameObject.SetActive(true);
        StartCoroutine(this.countDownCo());
    }

    public void returnToTitleScreen()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void returnSaveGame()
    {
        string scene;
        Vector2 position;

        if (this.player.GetComponent<PlayerTeleport>().getLastTeleport(out scene, out position))
        {
            this.colorGrading.saturation.value = 0;
            this.colorGrading.colorFilter.value = Color.white;

            this.gameObject.SetActive(false);
            this.UI.SetActive(true);
            //SceneManager.LoadSceneAsync(this.lastSavepoint);
            this.player.initPlayer();
            this.player.GetComponent<PlayerTeleport>().teleportPlayer(scene, position, true);
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
