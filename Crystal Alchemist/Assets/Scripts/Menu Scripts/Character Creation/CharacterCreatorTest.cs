using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorTest : MonoBehaviour
{
    [SerializeField]
    private SimpleSignal signal;

    [Button]
    private void UpdateCharacter()
    {
        this.signal.Raise();
    }
}
