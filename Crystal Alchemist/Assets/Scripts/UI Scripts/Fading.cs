using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private FloatValue transitionDuration;

    public void fade(bool fadeIn)
    {
        if (fadeIn) image.CrossFadeAlpha(0, this.transitionDuration.getValue(), true);
        else image.CrossFadeAlpha(1, this.transitionDuration.getValue(), true);
    }

}
