using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MaterialEvent : UnityEvent<Material>
{
}

public class MaterialSignalListener : MonoBehaviour
{
    public MaterialSignal signal;
    public MaterialEvent signalEventMaterial;

    public void OnSignalRaised(Material material)
    {
        this.signalEventMaterial.Invoke(material);
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
