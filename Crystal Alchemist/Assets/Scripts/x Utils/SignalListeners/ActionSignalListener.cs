using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ActionEvent : UnityEvent<UnityEvent>
{
}

public class ActionSignalListener : MonoBehaviour
{
    public ActionSignal signal;
    public ActionEvent signalEventString;

    public void OnSignalRaised(UnityEvent action)
    {
        this.signalEventString.Invoke(action);
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
