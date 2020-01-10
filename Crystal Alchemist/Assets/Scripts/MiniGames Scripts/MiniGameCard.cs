using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameCard : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private TextMeshProUGUI number;

    public void setValue(int value)
    {
        this.number.text = value + "";
    }

    public void show()
    {
        Utilities.UnityUtils.SetAnimatorParameter(this.anim, "Show");
    }
}
