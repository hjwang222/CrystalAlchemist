using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        Utilities.Audio.playSoundEffect(this.audioSource, audioClip);
    }
}
