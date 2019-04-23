using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    [Header("Dialog-Attribute")]
    public GameObject dialogBox;
    public TextMeshProUGUI textMesh;

    private bool showIt = false;
    private bool inputPossible = true;
    private List<string> texts = new List<string>();

    public AudioClip dialogSoundEffect;
    private AudioSource audioSource;

    [HideInInspector]
    public Player player;

    private int index = 0;

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
            if (Input.GetButtonDown("A-Button") && this.inputPossible)
            {
                if (this.index+2 < this.texts.Count)
                {
                    Utilities.playSoundEffect(this.audioSource, this.dialogSoundEffect);
                    this.index+=2;
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

    private void hideDialogBox()
    {
        this.showIt = false;
        this.index = 0;
        this.dialogBox.SetActive(false);

        if (this.player != null)
        {
            this.player.currentState = CharacterState.interact;
        }
    }

    private IEnumerator delayInputCO()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.3f);
        this.inputPossible = true;
    }

    public void showDialogBox(string text)
    {        
        this.showIt = true;
        this.texts.Clear();
        this.texts = cutText(text);

        if (this.player != null) this.player.currentState = CharacterState.inDialog;
        this.dialogBox.SetActive(true);
        showText();

        StartCoroutine(delayInputCO());
    }

    private List<string> cutText(string text)
    {
        List<string> result = new List<string>();
        
        string[] temp = text.Replace("\n", "\n ").Split(' ');
        string line = "";
        int i = 0;

        while (i < temp.Length)
        {
            string word = temp[i];

            if ((line + word).Length > 28 
                || i == temp.Length-1
                || word.Contains("\n"))
            {
                word = word.Replace("\n", "");
                line += word + " ";
                result.Add(line);
                line = "";
            }
            else if((line + word).Length <= 28)
            {
                line += word + " ";
            }
            i++;
        }

        return result;
    }

    private void showText()
    {
        if (this.index + 1 < this.texts.Count) this.textMesh.text = this.texts[this.index] + "\n" + this.texts[this.index + 1];
        else this.textMesh.text = this.texts[this.index];
    }

}
