using UnityEngine;
using Sirenix.OdinInspector;

public class AnimationUtil : MonoBehaviour
{
    public void playSoundEffect(AudioClip clip)
    {
        CustomUtilities.Audio.playSoundEffect(clip);
    }
}
