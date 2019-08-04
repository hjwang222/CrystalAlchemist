using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startMusic;
    public AudioClip loopMusic;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();

        if (this.startMusic != null)
        {
            this.audioSource.loop = false;
            this.audioSource.PlayOneShot(this.startMusic, GlobalValues.backgroundMusicVolume);

            StartCoroutine(playLoopMusic());
        }
        else
        {
            playLoop();
        }
    }

    public void changePitch()
    {
        this.audioSource.pitch = GlobalValues.backgroundMusicPitch;
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
            this.audioSource.volume = GlobalValues.backgroundMusicVolume;
            this.audioSource.loop = true;
            this.audioSource.Play();
        }
    }

    
}
