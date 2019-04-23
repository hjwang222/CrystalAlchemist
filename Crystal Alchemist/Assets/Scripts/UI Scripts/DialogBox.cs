using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    [Header("Dialog-Attribute")]
    public GameObject dialogBox;
    public TextMeshProUGUI textMesh;

    [HideInInspector]
    public AudioSource dialogSoundEffect;

    [HideInInspector]
    public Player player;

    private int index = 0;

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (this.player.showDialog)
        {
            if(index >= 2) hideDialog();

            showDialog(this.player.dialogText);

            if (Input.GetButtonDown("A-Button"))
            {
                index++;
            }           
        }
    }

    public void showDialog(string text)
    {
        //Zeige DialogBox an    
        
            if (this.player != null) this.player.currentState = CharacterState.inDialog;
            this.dialogBox.SetActive(true);
            this.textMesh.text = text;

           /* AudioSource audiosource = dialogBox.GetComponent<AudioSource>();
            AudioClip clip = audiosource.clip;
            Utilities.playSoundEffect(audiosource, clip);*/
        

        //OLD, muss besser gehen!
    }

    public void hideDialog()
    {
        this.index = 0;
        this.dialogBox.SetActive(false);

        if (this.player != null)
        {
            this.player.currentState = CharacterState.interact;
            this.player.showDialog = false;
            this.player.dialogText = "";
        }
    }
}
