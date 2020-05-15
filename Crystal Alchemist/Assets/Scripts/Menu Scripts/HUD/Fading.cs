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

    private void Start() => StartCoroutine(delayCo());    

    private void FadeIn() => this.image.CrossFadeAlpha(0, this.transitionDuration.getValue(), true);

    public void FadeOut() => this.image.CrossFadeAlpha(1, this.transitionDuration.getValue(), true);

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(0.1f);
        FadeIn();
    }
}
