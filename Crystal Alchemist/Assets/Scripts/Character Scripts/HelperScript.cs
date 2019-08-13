using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperScript : MonoBehaviour
{
    [SerializeField]
    private Character character;

    public void DestroyIt()
    {
        character.DestroyIt();
    }

    public void DestroyItCompletely()
    {
        character.DestroyItCompletely();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        character.PlaySoundEffect(clip);
    }

    public void updateSpeed(float addSpeed)
    {
        character.updateSpeed(addSpeed,false);
    }
}
