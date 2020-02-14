using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Signals/CharacterPresetSignal")]
public class CharacterPresetSignal : ScriptableObject
{
    public List<CharacterPresetSignalListener> listeners = new List<CharacterPresetSignalListener>();

    public void Raise(CharacterPreset preset)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            this.listeners[i].OnSignalRaised(preset);
        }
    }

    public void RegisterListener(CharacterPresetSignalListener listener)
    {
        this.listeners.Add(listener);
    }

    public void DeRegisterListener(CharacterPresetSignalListener listener)
    {
        this.listeners.Remove(listener);
    }
}
