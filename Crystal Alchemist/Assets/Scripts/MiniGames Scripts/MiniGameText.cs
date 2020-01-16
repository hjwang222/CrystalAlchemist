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
    private AudioClip audioClip;

    private float duration;
    private bool inputPossible = false;
    private float maxDuration = 3f;

    private void OnEnable()
    {
        if (this.audioClip != null && this.audioClip.length > 3) this.maxDuration = this.audioClip.length + 1f;

        this.duration = this.maxDuration;
        CustomUtilities.Audio.playSoundEffect(this.audioClip);
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
