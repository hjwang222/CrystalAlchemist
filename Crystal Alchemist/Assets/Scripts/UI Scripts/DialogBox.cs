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
    [SerializeField]
    private GameObject cursor;

    private bool showIt = false;
    private bool inputPossible = true;
    private List<string> texts = new List<string>();
    private AudioSource audioSource;
    private Player player;
    private int index = 0;
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

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        this.cursor.SetActive(false);
    }


    public void next(int index)
    {
        this.index += index;

        if (this.index < 0) this.index = 0;

        if (this.index < this.texts.Count)
        {
            //Blättere weiter                                       
            showText();
        }
        else
        {
            hideDialogBox();
        }
    }

    #endregion


    #region Funktionen (Show, Hide, Text)

    public void showDialogBox(string text)
    {
        //Zeige die DialogBox (Signal)
        this.cursor.SetActive(true);
        this.showIt = true;
        this.texts.Clear();
        this.texts = formatText(text);

        if (this.player != null) this.player.currentState = CharacterState.inDialog;
        this.dialogBox.SetActive(true);
        showText();
    }

    private void hideDialogBox()
    {
        //Blende DialogBox aus
        this.player.delay(CharacterState.interact);
        this.cursor.SetActive(false);
        this.showIt = false;
        this.index = 0;
        this.dialogBox.SetActive(false);        
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
                || i == temp.Length - 1
                || word.Contains("\n"))
            {
                word = word.Replace("\n", "");
                line += word + " ";
                result.Add(line);
                line = "";
            }
            else if ((line + word).Length <= this.maxLength)
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
