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

    [SerializeField]
    private Color colorFadeIn = Color.black;

    [SerializeField]
    private Color colorFadeOut = Color.white;

    public void fade(bool fadeIn)
    {
        if (fadeIn)
        {            
            this.image.CrossFadeAlpha(0, this.transitionDuration.getValue(), true);
        }
        else
        {            
            this.image.CrossFadeAlpha(1, this.transitionDuration.getValue(), true);
        }        
    }

}
