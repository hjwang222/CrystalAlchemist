using System.Collections;
using UnityEngine;

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

    private void Start()
    {
        GameEvents.current.OnSubmit += Disable;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnSubmit -= Disable;
    }

    private void OnEnable()
    {
        if (this.audioClip != null && this.audioClip.length > 3) this.maxDuration = this.audioClip.length + 1f;

        this.duration = this.maxDuration;
        AudioUtil.playSoundEffect(this.audioClip);
        StartCoroutine(delayInput());
        StartCoroutine(DisableCo());
    }

    public void Disable()
    {
        if (this.inputPossible)
        {
            if (this.showDialogBox) this.miniGameUI.showDialog(); //WIN or LOSE
            else this.miniGameUI.startRound();
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator delayInput()
    {
        this.inputPossible = false;
        yield return new WaitForSeconds(0.3f);
        this.inputPossible = true;
    }

    private IEnumerator DisableCo()
    {
        yield return new WaitForSeconds(this.maxDuration);
        Disable();
    }
}
