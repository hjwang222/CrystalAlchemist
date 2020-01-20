using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip startMusic;

    [SerializeField]
    private AudioClip loopMusic;

    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        this.volume = GlobalValues.backgroundMusicVolume;
        this.audioSource = this.GetComponent<AudioSource>();
        this.audioSource.volume = this.volume;

        startPlaying();
    }

    public void changePitch()
    {
        this.audioSource.pitch = GlobalValues.backgroundMusicPitch;        
    }

    public void changeVolume(float volume)
    {
        this.audioSource.volume = volume;
        this.volume = volume;
    }

    public void startPlaying()
    {
            stopMusic();

            if (this.startMusic != null)
            {                
                this.audioSource.loop = false;
                this.audioSource.PlayOneShot(this.startMusic);

                StartCoroutine(playLoopMusic());
            }
            else 
            {                                
                playLoop();
            }
        
    }

    public void stopMusic()
    {
        this.audioSource.Stop();
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
            this.audioSource.volume = this.volume;
            this.audioSource.clip = this.loopMusic;
            this.audioSource.loop = true;
            this.audioSource.Play();
        }
    }

    
}
