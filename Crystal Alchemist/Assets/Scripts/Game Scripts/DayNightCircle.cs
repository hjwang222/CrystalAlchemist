using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class DayNightCircle : MonoBehaviour
{
    [SerializeField]
    private Color day;

    [SerializeField]
    private Color night;

    [SerializeField]
    private Light2D light;

    public bool changeColor = false;

    private Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(changeColor) StartCoroutine(lerpColor(light, day, night, 10f));
    }

    IEnumerator lerpColor(Light2D light, Color fromColor, Color toColor, float duration)
    {
        this.changeColor = false;
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
    }
}
