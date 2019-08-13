using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{

    [SerializeField]
    private bool value;

    public bool getValue()
    {
        return this.value;
    }

    public void setValue(bool value)
    {
        this.value = value;
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }

}
