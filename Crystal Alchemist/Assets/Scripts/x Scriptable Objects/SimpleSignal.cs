using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SimpleSignal : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        for(int i = listeners.Count-1; i >= 0; i--)
        {
            this.listeners[i].OnSignalRaised();
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
