using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public SimpleSignal signal;
    public UnityEvent signalEvent;

    public void OnSignalRaised()
    {
        this.signalEvent.Invoke();
    }

    private void OnEnable()
    {
        try
        {
            signal.RegisterListener(this);
        }
        catch 
        {
            string elem = this.ToString();
        }
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
