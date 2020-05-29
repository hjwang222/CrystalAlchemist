using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip startMusic;

    [SerializeField]
    private AudioClip loopMusic;

    private void Start() => StartCoroutine(musicCo());

    private IEnumerator musicCo()
    {
        yield return new WaitForEndOfFrame();
        MusicEvents.current.PlayMusic(this.startMusic, this.loopMusic);
    }
}
