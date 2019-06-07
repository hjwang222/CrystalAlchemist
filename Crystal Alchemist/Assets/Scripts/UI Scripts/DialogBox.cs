using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    #region Attribute
    [Header("Dialog-Attribute")]
    [Tooltip("DialogBox Child-Objekt")]
    public GameObject dialogBox;
    [Tooltip("DialogBox Text-Objekt")]
    public TextMeshProUGUI textMesh;
    [Tooltip("Sound der Dialogbox")]
    public AudioClip dialogSoundEffect;

    private bool showIt = false;
    private bool inputPossible = true;
    private List<string> texts = new List<string>();
    private AudioSource audioSource;
    private Player player;
    private int index = 0;
    private float delay = 0.3f;
    private int maxLength = 28;
    #endregion


    #region Unity Funktionen (Start, Update)
    private void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (this.showIt)
        {
            //Wenn DialogBox aktiv ist
            //TODO: B-Button einfügen

            if (this.inputPossible && (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")))
            {
                if (Input.GetButtonDown("Submit")) this.index += 2;
                else if (Input.GetButtonDown("Cancel")) this.index -= 2;

                if (this.index < 0) this.index = 0;

                if (this.index < this.texts.Count)
                {
                    //Blättere weiter                                       
                    showText();
                    StartCoroutine(delayInputCO());
                }
                else
                {
                    hideDialogBox();
                }
            }
        }
    }

    #endregion


    #region Funktionen (Show, Hide, Text)

    public void showDialogBox(string text)
    {        
        //Zeige die DialogBox (Signal)
        this.showIt = true;
        this.texts.Clear();
        this.texts = formatText(text);

        if (this.player != null) this.player.currentState = CharacterState.inDialog;
        this.dialogBox.SetActive(true);
        showText();

        StartCoroutine(delayInputCO());
    }

    private void hideDialogBox()
    {
        //Blende DialogBox aus
        this.showIt = false;
        this.index = 0;
        this.dialogBox.SetActive(false);

        StartCoroutine(delayInputPlayerCO());
    }

    private IEnumerator delayInputCO()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(this.delay);
        this.inputPossible = true;
    }

    private IEnumerator delayInputPlayerCO()
    {        
        //Damit der Spieler nicht gleich wieder die DialogBox aktiviert : /
        yield return new WaitForSeconds(this.delay);

        if (this.player != null)
        {
            this.player.currentState = CharacterState.interact;
        }
    }

    private List<string> formatText(string text)
    {
        List<string> result = new List<string>();
        
        string[] temp = text.Replace("\n", "\n ").Split(' ');
        string line = "";
        int i = 0;

        while (i < temp.Length)
        {
            string word = temp[i];

            if ((line + word).Length > this.maxLength
                || i == temp.Length-1
                || word.Contains("\n"))
            {
                word = word.Replace("\n", "");
                line += word + " ";
                result.Add(line);
                line = "";
            }
            else if((line + word).Length <= this.maxLength)
            {
                line += word + " ";
            }
            i++;
        }

        return result;
    }

    private void showText()
    {
        Utilities.playSoundEffect(this.audioSource, this.dialogSoundEffect);

        if (this.index + 1 < this.texts.Count) this.textMesh.text = this.texts[this.index] + "\n" + this.texts[this.index + 1];
        else this.textMesh.text = this.texts[this.index];
    }

    #endregion

}
