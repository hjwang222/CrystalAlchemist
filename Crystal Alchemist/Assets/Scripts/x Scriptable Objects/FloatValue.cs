using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private float startValue;

    [SerializeField]
    private float pitch = 1f;

    [HideInInspector]
    public float value;
    [HideInInspector]
    public float pitchValue;

    public void OnAfterDeserialize()
    {
        this.value = this.startValue;
        this.pitchValue = this.pitch;
    }

    public void OnBeforeSerialize() { }

}
