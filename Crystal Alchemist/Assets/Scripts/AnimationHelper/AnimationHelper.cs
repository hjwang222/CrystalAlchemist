using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject mainObject;

    public void PlaySoundEffect(AudioClip audioClip)
    {
        AudioUtil.playSoundEffect(this.mainObject, audioClip);
    }

    public void DestroyIt()
    {
        Destroy(this.mainObject);
    }
}
