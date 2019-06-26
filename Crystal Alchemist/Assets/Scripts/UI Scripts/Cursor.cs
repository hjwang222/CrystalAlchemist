using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundEffect;
    private AudioSource audioSource;
    private bool isPlaying = false;

    private void Awake()
    {
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    public void playSoundEffect()
    {
        if (!this.isPlaying)
        {
            StartCoroutine(playAudioEffect());          
        }
    }

    private IEnumerator playAudioEffect()
    {
        this.isPlaying = true;
        Utilities.playSoundEffect(this.audioSource, this.soundEffect);
        float length = this.soundEffect.length;
        yield return new WaitForSeconds(length);
        this.isPlaying = false;
    }
}
