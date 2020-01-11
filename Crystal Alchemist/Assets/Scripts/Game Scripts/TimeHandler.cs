using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private TimeValue timeValue;

    private void Start()
    {
        int minute = 0;
        int hour = 0;

        CustomUtilities.Format.getStartTime(this.timeValue.factor, out hour, out minute);
        this.timeValue.setTime(minute, hour);
        this.timeValue.factor = this.timeValue.normalFactor;
    }

    // Update is called once per frame
    void Update()
    {
        this.timeValue.setTime(Time.deltaTime);
    }
}
