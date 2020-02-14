using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterPresetEvent : UnityEvent<CharacterPreset>
{
}

public class CharacterPresetSignalListener : MonoBehaviour
{
    public CharacterPresetSignal signal;
    public CharacterPresetEvent signalEventColor;

    public void OnSignalRaised(CharacterPreset preset)
    {
        this.signalEventColor.Invoke(preset);
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
