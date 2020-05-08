using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private TimeValue timeValue;

    private void FixedUpdate() => this.timeValue.setTime(Time.fixedDeltaTime);
}
