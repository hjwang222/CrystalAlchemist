using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    
    [SerializeField]
    private float value;

    public float getValue()
    {
        return this.value;
    }

    public void OnAfterDeserialize() { }    

    public void OnBeforeSerialize() { }

}
