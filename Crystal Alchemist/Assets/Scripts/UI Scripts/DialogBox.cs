using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    #region Attribute
    [Header("Dialog-Attribute")]
    [SerializeField]
    private PlayerStats playerStats;
    [Tooltip("DialogBox Child-Objekt")]
    [SerializeField]
    private GameObject dialogBox;
    [Tooltip("DialogBox Text-Objekt")]
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [Tooltip("Sound der Dialogbox")]
    [SerializeField]
    private AudioClip dialogSoundEffect;
    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private GameObject controls;

    private bool showIt = false;
    private bool inputPossible = true;
    private List<string> texts = new List<string>();
    private AudioSource audioSource;
    private Player player;
    private int index = 0;
    private int maxLength = 28;
    #endregion


    #region Unity Funktionen (Start, Update)
    private void Awake()
    {
        this.player = this.playerStats.player;
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
        this.controls.SetActive(false);
        this.dialogBox.SetActive(true);
        showText();
    }

    private void hideDialogBox()
    {
        //Blende DialogBox aus
        this.player.delay(CharacterState.interact);
        this.controls.SetActive(true);
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
        CustomUtilities.Audio.playSoundEffect(this.audioSource, this.dialogSoundEffect);

        if (this.index + 1 < this.texts.Count) this.textMesh.text = this.texts[this.index] + "\n" + this.texts[this.index + 1];
        else this.textMesh.text = this.texts[this.index];
    }

    #endregion

}
