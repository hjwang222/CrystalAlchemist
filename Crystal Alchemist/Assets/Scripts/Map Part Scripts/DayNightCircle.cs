using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class DayNightCircle : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Experimental.Rendering.Universal.Light2D light;

    [SerializeField]
    private TimeValue timeValue;

    [SerializeField]
    private bool isActive = true;

    private bool isRunning = false;

    private void Start()
    {
        if(this.isActive) StartCoroutine(startCo());  
    }

    IEnumerator startCo()
    {
        yield return null;
        this.light.color = this.timeValue.GetColor();
    }

    public void changeColor()
    {
        if (this.isActive)
        {
            Color newColor = this.timeValue.GetColor();
            float duration = this.timeValue.factor * (this.timeValue.update - 1);
            if (newColor != light.color && !this.isRunning) StartCoroutine(lerpColor(light, light.color, newColor, duration));
        }
    }

    IEnumerator lerpColor(UnityEngine.Experimental.Rendering.Universal.Light2D light, Color fromColor, Color toColor, float duration)
    {
        this.isRunning = true;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            float colorTime = counter / duration;

            //Change color
            light.color = Color.Lerp(fromColor, toColor, counter / duration);
            
            //Wait for a frame
            yield return null;
        }

        this.isRunning = false;
    }
}
