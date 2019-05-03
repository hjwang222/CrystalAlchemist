using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ResourceSignal : ScriptableObject
{
    public List<ResourceSignalListener> listeners = new List<ResourceSignalListener>();

    public void Raise(ResourceType type, float amount)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            this.listeners[i].OnSignalRaised(type, amount);
        }
    }

    public void RegisterListener(ResourceSignalListener listener)
    {
        this.listeners.Add(listener);
    }

    public void DeRegisterListener(ResourceSignalListener listener)
    {
        this.listeners.Remove(listener);
    }
}
