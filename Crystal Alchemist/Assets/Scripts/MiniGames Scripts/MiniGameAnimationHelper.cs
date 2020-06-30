using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameAnimationHelper : MonoBehaviour
{
    [SerializeField]
    private MiniGameRound miniGameRound;

    public void Check()
    {
        this.miniGameRound.Check();
    }
}
