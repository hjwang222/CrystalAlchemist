using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private Character character;
    public void setCharacter(Character character)
    {
        this.character = character;
    }

    public void DestroyIt()
    {
        this.character.DestroyIt();
        Destroy(this.gameObject, 0.1f);
    }

    public void DestoryItCompletely()
    {
        this.character.DestroyItCompletely();
        Destroy(this.gameObject, 0.1f);
    }

    public void PlayDeathSoundEffect(AudioClip soundEffect)
    {
        CustomUtilities.Audio.playSoundEffect(this.audioSource, soundEffect);
    }
}
