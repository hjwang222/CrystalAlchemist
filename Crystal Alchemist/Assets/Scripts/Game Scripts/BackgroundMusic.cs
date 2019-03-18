using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startMusic;
    public AudioClip loopMusic;
    public FloatValue backgroundMusicVolume;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();

        if (this.startMusic != null)
        {
            this.audioSource.loop = false;
            this.audioSource.PlayOneShot(this.startMusic, this.backgroundMusicVolume.value);

            StartCoroutine(playLoopMusic());
        }
        else
        {
            playLoop();
        }
    }

    private IEnumerator playLoopMusic()
    {
        yield return new WaitForSecondsRealtime(this.startMusic.length);
        playLoop();
    }

    private void playLoop()
    {
        if (this.loopMusic != null)
        {
            this.audioSource.clip = this.loopMusic;
            this.audioSource.volume = this.backgroundMusicVolume.value;
            this.audioSource.loop = true;
            this.audioSource.Play();
        }
    }

    
}
