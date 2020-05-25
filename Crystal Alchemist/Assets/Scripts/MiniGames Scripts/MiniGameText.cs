using System.Collections;
using UnityEngine;

public class MiniGameText : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private SimpleSignal signal;

    private float duration;
    private bool inputPossible = false;
    private float maxDuration = 3f;


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
            this.signal.Raise();
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
