using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Time")]
public class TimeValue : ScriptableObject, ISerializationCallbackReceiver
{
    private float seconds = 0;

    [SerializeField]
    private int minute = 0;

    [SerializeField]
    private int hour = 12;

    public float normalFactor = 1;

    public int update = 30;
    public bool night;
    public float factor = 1;    

    [SerializeField]
    private Gradient colorGradient;

    [SerializeField]
    private SimpleSignal signal;

    public Color GetColor()
    {
        float temp = (float)(100f / 24f / 60f);
        float value = (float)((this.hour * 60) + this.minute);
        float percentage = (temp * value) / 100;
        return this.colorGradient.Evaluate(percentage);
    }


    public int getMinute()
    {
        return this.minute;
    }

    public int getHour()
    {
        return this.hour;
    }

    public void setTime(int minute, int hour)
    {
        this.minute = minute;
        this.hour = hour;
        updateTimer();
    }

    public void setTime(float seconds)
    {
        this.seconds += seconds;
        updateTimer();
    }

    private void updateTimer()
    {
        if (hour < 18 && hour > 6) this.night = false;
        else this.night = true;

        if (this.seconds >= this.factor)
        {
            this.minute += 1;
            this.seconds = 0;

            if (this.minute % this.update == 0)
            {
                this.signal.Raise();
            }

            if (this.minute >= 60)
            {
                this.minute = 0;
                this.hour += 1;

                if (this.hour >= 24) this.hour = 0;
            }
        }
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}
