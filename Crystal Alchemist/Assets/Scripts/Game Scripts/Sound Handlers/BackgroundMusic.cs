using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip startMusic;

    [SerializeField]
    private AudioClip loopMusic;

    [SerializeField]
    private float fadeIn = 2f;

    [SerializeField]
    private float fadeOut;

    private void Start() => MusicEvents.current.PlayMusic(this.startMusic, this.loopMusic, this.fadeIn);    

    public void PlayMusic(AudioClip music)
    {
        MusicEvents.current.StopMusic(this.fadeOut);
        MusicEvents.current.PlayMusic(null, music, this.fadeIn);
    }
}
