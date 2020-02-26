using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Signals/MaterialSignal")]
public class MaterialSignal : ScriptableObject
{
    public List<MaterialSignalListener> listeners = new List<MaterialSignalListener>();

    public void Raise(Material material)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            this.listeners[i].OnSignalRaised(material);
        }
    }

    public void RegisterListener(MaterialSignalListener listener)
    {
        this.listeners.Add(listener);
    }

    public void DeRegisterListener(MaterialSignalListener listener)
    {
        this.listeners.Remove(listener);
    }
}
