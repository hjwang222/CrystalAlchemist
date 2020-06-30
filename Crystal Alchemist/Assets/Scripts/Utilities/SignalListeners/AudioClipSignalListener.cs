using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Single4 : UnityEvent<AudioClip>
{
}

public class AudioClipSignalListener : MonoBehaviour
{
    public AudioClipSignal signal;
    public Single4 signalEventBool;

    public void OnSignalRaised(AudioClip value)
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
