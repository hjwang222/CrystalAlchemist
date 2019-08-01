using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightInterior : MonoBehaviour
{
    [SerializeField]
    private GameObject light;

    [SerializeField]
    private TimeValue timeValue;

    private void Start()
    {
        switchInteriorLights();
    }

    public void switchInteriorLights()
    {        
        if (this.timeValue.night) light.SetActive(true);
        else if (!this.timeValue.night) light.SetActive(false);
    }
}
