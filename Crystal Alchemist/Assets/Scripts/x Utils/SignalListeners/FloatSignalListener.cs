using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float>{}

public class FloatSignalListener : MonoBehaviour
{
    public FloatSignal signal;
    public FloatEvent signalEventBool;

    public void OnSignalRaised(float value) => this.signalEventBool.Invoke(value);
    
    private void OnEnable() => signal.RegisterListener(this);    

    private void OnDisable() => signal.DeRegisterListener(this);    
}
