using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private float duration = 2f;







    public void fade(bool fadeIn)
    {
        if (fadeIn) image.CrossFadeAlpha(0, duration, true);
        else image.CrossFadeAlpha(1, duration, true);
    }

}
