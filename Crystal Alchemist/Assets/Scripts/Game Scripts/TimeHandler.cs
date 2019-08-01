using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private TimeValue timeValue;
    
    private void Start()
    {
        PlayerData data = SaveSystem.loadPlayer();
        if (data != null)
        {
            this.timeValue.setTime(data.minute, data.hour);
        }
        else
        {
            this.timeValue.setTime(System.DateTime.Now.Minute, System.DateTime.Now.Hour);
        }

        this.timeValue.init();
    }

    // Update is called once per frame
    void Update()
    {
        this.timeValue.setTime(Time.deltaTime);
    }
}
