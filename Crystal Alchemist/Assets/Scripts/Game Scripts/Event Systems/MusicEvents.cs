using System;
using UnityEngine;

public class MusicEvents : MonoBehaviour
{
    public static MusicEvents current;

    private void Awake() => Initialize();

    private void Initialize()
    {
        current = this;
    }

    public Action<AudioClip, AudioClip> OnBackgroundMusicPlayed;
    public Action<AudioClip> OnMusicPlayed;
    public Action<AudioClip, bool> OnMusicPlayedResume;
    public Action OnMusicPaused;
    public Action<float> OnMusicVolumeChanged;
    public Action OnMusicResumed;
    public Action OnMusicRestart;
    public Action OnMusicStopped;

    public void PlayMusic(AudioClip start, AudioClip loop)
        => this.OnBackgroundMusicPlayed?.Invoke(start, loop);
    public void PlayMusic(AudioClip music) => this.OnMusicPlayed?.Invoke(music);
    public void PlayMusic(AudioClip music, bool resume) => this.OnMusicPlayedResume?.Invoke(music, resume);
    public void PauseMusic() => this.OnMusicPaused?.Invoke();
    public void ChangeVolume(float value) => this.OnMusicVolumeChanged?.Invoke(value);
    public void ResumeMusic() => this.OnMusicResumed?.Invoke();
    public void RestartMusic() => this.OnMusicRestart?.Invoke();
    public void StopMusic() => this.OnMusicStopped?.Invoke();
}
