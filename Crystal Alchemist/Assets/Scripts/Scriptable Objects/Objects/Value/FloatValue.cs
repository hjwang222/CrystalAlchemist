using UnityEngine;

[CreateAssetMenu(menuName = "Values/FloatValue")]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{    
    [SerializeField]
    private float value;

    public float GetValue()
    {
        return this.value;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void OnAfterDeserialize() { }    

    public void OnBeforeSerialize() { }

}
