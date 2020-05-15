using UnityEngine;

public static class AudioHandler
{
    public static void playSound(AudioClip clip)
    {
        GameObject temp = new GameObject();
        AudioSource source = temp.AddComponent<AudioSource>();
        source.PlayOneShot(clip);
    }
}
