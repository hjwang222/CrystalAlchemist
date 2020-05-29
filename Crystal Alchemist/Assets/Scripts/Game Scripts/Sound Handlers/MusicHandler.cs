using System.Collections;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    private MusicObject backgroundMusic;
    private MusicObject tempMusic;

    private void Start()
    {
        MusicEvents.current.OnBackgroundMusicPlayed += PlayMusic;
        MusicEvents.current.OnMusicPlayed += PlayMusic;
        MusicEvents.current.OnMusicPlayedResume += PlayMusic;
        MusicEvents.current.OnMusicPaused += PauseMusic;
        MusicEvents.current.OnMusicVolumeChanged += ChangeVolume;
        MusicEvents.current.OnMusicResumed += ResumeMusic;
        MusicEvents.current.OnMusicRestart += RestartMusic;
        MusicEvents.current.OnMusicStopped += StopMusic;
    }

    private void OnDestroy()
    {
        MusicEvents.current.OnBackgroundMusicPlayed -= PlayMusic;
        MusicEvents.current.OnMusicPlayed -= PlayMusic;
        MusicEvents.current.OnMusicPlayedResume -= PlayMusic;
        MusicEvents.current.OnMusicPaused -= PauseMusic;
        MusicEvents.current.OnMusicVolumeChanged -= ChangeVolume;
        MusicEvents.current.OnMusicResumed -= ResumeMusic;
        MusicEvents.current.OnMusicRestart -= RestartMusic;
        MusicEvents.current.OnMusicStopped -= StopMusic;
    }

    private void PlayMusic(AudioClip start, AudioClip loop)
    {
        if (loop == null) return;
        GameObject newGameObject = new GameObject("Main Music");
        MusicObject musicObject = newGameObject.AddComponent<MusicObject>();
        musicObject.Instantiate(start, loop);
        this.backgroundMusic = musicObject;
    }

    private void PlayMusic(AudioClip music)
    {
        PlayMusic(music, true);
    }

    private void PlayMusic(AudioClip music, bool resume)
    {
        if (music == null) return;
        PauseMusic();

        if(this.tempMusic != null)
        {
            Destroy(this.tempMusic.gameObject);
            StopCoroutine(delayCo(music.length));
        }

        GameObject newGameObject = new GameObject("Music");
        MusicObject musicObject = newGameObject.AddComponent<MusicObject>();
        musicObject.Instantiate(music);
        this.tempMusic = musicObject;

        if(resume) StartCoroutine(delayCo(music.length));
    }

    private void PauseMusic()
    {
        if (this.backgroundMusic != null) this.backgroundMusic.Pause();
    }

    private void ResumeMusic()
    {
        if (this.backgroundMusic != null) this.backgroundMusic.Resume();
    }

    private void RestartMusic()
    {
        if (this.backgroundMusic != null) this.backgroundMusic.RestartMusic();
    }

    private void ChangeVolume(float volume)
    {
        if (this.backgroundMusic != null) this.backgroundMusic.ChangeVolume(volume);
    }

    private void StopMusic()
    {
        if (this.backgroundMusic != null) this.backgroundMusic.Stop();
    }

    private IEnumerator delayCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResumeMusic();
    }
}

public class MusicObject : MonoBehaviour
{
    private AudioClip startMusic;
    private AudioClip loopMusic;
    private AudioSource audioSource;
    private float volume;
    private bool loop = false;

    public void Instantiate(AudioClip startMusic, AudioClip loopMusic)
    {
        this.loop = true;
        this.startMusic = startMusic;
        this.loopMusic = loopMusic;
    }

    public void Instantiate(AudioClip music)
    {
        this.loopMusic = music;
        Destroy(this.gameObject, music.length);
    }

    private void Start()
    {
        this.volume = MasterManager.settings.backgroundMusicVolume;

        this.audioSource = this.gameObject.AddComponent<AudioSource>();
        this.audioSource.volume = this.volume;        

        Initialize();
    }
    
    public void RestartMusic()
    {
        this.audioSource.Stop();
        Initialize();
    }

    public void ChangeVolume(float volume)
    {
        this.audioSource.volume = volume;
    }

    public void Pause()
    {
        this.audioSource.Pause();
    }

    public void Stop()
    {
        this.audioSource.Stop();
    }

    public void Resume()
    {
        this.audioSource.UnPause();
    }

    private void Initialize()
    {
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

    private void playLoop()
    {
        if (this.loopMusic != null)
        {
            this.audioSource.volume = this.volume;
            this.audioSource.clip = this.loopMusic;
            this.audioSource.loop = this.loop;
            this.audioSource.Play();
        }
    }

    private IEnumerator playLoopMusic()
    {
        yield return new WaitForSecondsRealtime(this.startMusic.length);
        playLoop();
    }
}
