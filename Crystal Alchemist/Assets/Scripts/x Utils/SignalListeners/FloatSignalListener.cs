using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Single3 : UnityEvent<float>
{
}

public class FloatSignalListener : MonoBehaviour
{
    public FloatSignal signal;
    public Single3 signalEventBool;

    public void OnSignalRaised(float value)
    {
        this.signalEventBool.Invoke(value);
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
