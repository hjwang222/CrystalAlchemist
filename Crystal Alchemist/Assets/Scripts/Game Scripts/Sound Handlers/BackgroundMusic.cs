using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private bool playOnAwake = true;

    [SerializeField]
    private AudioClip startMusic;

    [SerializeField]
    private AudioClip loopMusic;

    [SerializeField]
    private float fadeIn = 2f;

    [SerializeField]
    private float fadeOut;

    private void Start()
    {
        if (this.playOnAwake) PlayMusic();
    }

    public void PlayMusic()
    {
        StopMusic();
        MusicEvents.current.PlayMusic(this.startMusic, this.loopMusic, this.fadeIn);
    }

    public void StopMusic() => MusicEvents.current.StopMusic(this.fadeOut);

    public void PlayMusic(AudioClip music)
    {
        StopMusic();
        MusicEvents.current.PlayMusic(null, music, this.fadeIn);
    }
}
