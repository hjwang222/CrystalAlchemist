using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Signals/SimpleSignal")]
public class SimpleSignal : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        List<SignalListener> activelisteners = new List<SignalListener>();

        for (int i = listeners.Count-1; i >= 0; i--)
        {
            if (this.listeners[i] != null)
            {
                this.listeners[i].OnSignalRaised();
            }
        }
    }

    public void RegisterListener (SignalListener listener)
    {
        this.listeners.Add(listener);
    }

    public void DeRegisterListener (SignalListener listener)
    {
        this.listeners.Remove(listener);
    }
}
