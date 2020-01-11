using UnityEngine;
using Sirenix.OdinInspector;

public class AnimationUtil : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;


    [ButtonGroup("Add AudioSource")]
    private void addAudiosource()
    {
        this.audioSource = this.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    public void playSoundEffect(AudioClip clip)
    {
        CustomUtilities.Audio.playSoundEffect(this.audioSource, clip);
    }
}
