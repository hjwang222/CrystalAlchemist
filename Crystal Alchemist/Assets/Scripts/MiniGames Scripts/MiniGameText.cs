using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameText : MonoBehaviour
{
    [SerializeField]
    private MiniGameUI miniGameUI;

    [SerializeField]
    private bool showDialogBox = false;

    [SerializeField]
    private float maxDuration = 0;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip audioClip;

    private float duration;
    private bool inputPossible = false;

    private void OnEnable()
    {
        this.duration = this.maxDuration;
        Utilities.Audio.playSoundEffect(this.audioSource, this.audioClip);
        StartCoroutine(delayInput());
    }

    private void Update()
    {        
        if (this.duration > 0) this.duration -= Time.deltaTime;
        else if ((this.duration <= 0 && this.maxDuration > 0)
             || (this.inputPossible && Input.GetButtonDown("Submit"))) Disable();
    }

    public void Disable()
    {
        if (this.showDialogBox) showDialog(); //WIN or LOSE
        else this.miniGameUI.startRound();
        this.gameObject.SetActive(false);
    }

    private IEnumerator delayInput()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.3f);
        this.inputPossible = true;
    }

   public void showDialog()
    {
        this.miniGameUI.showDialog();
    }

}
