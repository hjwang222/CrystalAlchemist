using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject mainObject;

    public void PlaySoundEffect(AudioClip audioClip)
    {
        Utilities.Audio.playSoundEffect(this.audioSource, audioClip);
    }

    public void DestroyIt()
    {
        Destroy(this.mainObject);
    }
}
