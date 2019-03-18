using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("Dialog-Attribute")]
    public GameObject dialogBox;
    public TextMeshProUGUI textMesh;

    [HideInInspector]
    public AudioSource dialogSoundEffect;

    public FloatValue soundEffectVolume;

    [HideInInspector]
    public PlayerMovement player;

    public void showDialog(GameObject character, string text)
    {
        //Zeige DialogBox an
        this.player = character.GetComponent<PlayerMovement>();
        
            if (this.dialogBox.activeInHierarchy)
            {
                this.dialogBox.SetActive(false);
                if (this.player != null) this.player.currentState = CharacterState.interact;
            }
            else
            {
                if (this.player != null) this.player.currentState = CharacterState.inDialog;
                this.dialogBox.SetActive(true);
                this.textMesh.text = text;

                AudioSource audiosource = dialogBox.GetComponent<AudioSource>();
                AudioClip clip = audiosource.clip;
                Utilities.playSoundEffect(audiosource, clip, this.soundEffectVolume);

        }

        //OLD, muss besser gehen!
    }
}
