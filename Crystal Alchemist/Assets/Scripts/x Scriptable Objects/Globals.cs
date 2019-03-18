using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Globals : ScriptableObject, ISerializationCallbackReceiver
{
    public Color color = Color.white;

    #region Functions  
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
    #endregion
}
