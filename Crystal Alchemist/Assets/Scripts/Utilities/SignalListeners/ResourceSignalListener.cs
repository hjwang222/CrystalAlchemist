﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SingleTwo : UnityEvent<CostType, float>
{
}

public class ResourceSignalListener : MonoBehaviour
{
    public ResourceSignal signal;
    public SingleTwo signalEventString;

    public void OnSignalRaised(CostType type, float amount)
    {
        this.signalEventString.Invoke(type, amount);
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