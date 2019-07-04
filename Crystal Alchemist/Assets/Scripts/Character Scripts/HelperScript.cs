using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperScript : MonoBehaviour
{
    public Character character;

    public void DestroyIt()
    {
        character.DestroyIt();
    }

    public void PlayDeathSoundEffect()
    {
        character.PlayDeathSoundEffect();
    }
}
