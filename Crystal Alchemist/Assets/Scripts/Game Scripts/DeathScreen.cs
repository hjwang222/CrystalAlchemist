using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class DeathScreen : MonoBehaviour
{
    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private PlayerTeleportList playerTeleportList;

    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private TeleportStats nextTeleport;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private PlayerStats playerStats;

    [SerializeField]
    [BoxGroup("Mandatory")]
    private SimpleSignal stopMusic;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private StringValue loadGame;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private AudioClip deathMusic;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private AudioSource audioSource;

    [BoxGroup("Mandatory")]
    [SerializeField]
    private CustomCursor cursor;

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
    private float textDelay = 0.1f;

    [BoxGroup("Time")]
    [SerializeField]
    private float cursorDelay = 2f;

    [BoxGroup("Time")]
    [SerializeField]
    private float inputDelay = 3f;

    [BoxGroup("Signals")]
    [SerializeField]
    private ActionSignal fadeSignal;

    private string currentText;
    private string fullText;
    private bool skip = false;
    private bool inputPossible = false; 

    private void init()
    {
        this.skip = false;
        this.cursor.gameObject.SetActive(false);
        this.returnSavePoint.SetActive(false);
        this.returnTitleScreen.SetActive(false);
        this.countDown.gameObject.SetActive(false);
        this.textField.gameObject.SetActive(false);
        SceneManager.UnloadSceneAsync("UI");
        StartCoroutine(delayCo(this.inputDelay));
    }

    private IEnumerator delayCo(float delay)
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(delay);
        this.inputPossible = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && this.inputPossible) this.skip = true;
    }

    private void OnEnable()
    {
        init();
        this.stopMusic.Raise();
        this.fadeSignal.Raise(showText);
    }

    private void showText()
    {
        playMusic(this.deathMusic);
        this.textField.gameObject.SetActive(true);
        ShowText(this.textDelay);
    }

    public void playMusic(AudioClip clip)
    {
        AudioUtil.playSoundEffect(clip, this.audioSource, MasterManager.settings.backgroundMusicVolume);
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
        if (this.playerTeleportList.TeleportEnabled()) this.returnSavePoint.SetActive(true);

        this.countDown.gameObject.SetActive(true);
        StartCoroutine(this.countDownCo());
    }

    public void returnToTitleScreen()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void returnSaveGame()
    {
        if (this.playerTeleportList.TeleportEnabled())
        {
            this.nextTeleport.SetValue(this.playerTeleportList.GetStats(0));
            SceneManager.LoadScene(this.nextTeleport.scene);
        }
    }

    private IEnumerator ShowTextCo(float delay)
    {
        for(int i = 0; i <= this.fullText.Length; i++)
        {           
            this.currentText = this.fullText.Substring(0, i);

            if (skip)
            {
                this.currentText = this.fullText;
                i = this.fullText.Length;
            }

            this.textField.text = this.currentText;

            if (i >= this.fullText.Length)
            {
                StartCoroutine(showButtonCo(this.cursorDelay));
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
}
