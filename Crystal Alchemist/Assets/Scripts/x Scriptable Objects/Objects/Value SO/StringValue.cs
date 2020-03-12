using UnityEngine;

[CreateAssetMenu(menuName = "Values/StringValue")]
public class StringValue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private string value;

    public string getValue()
    {
        return this.value;
    }

    public void setValue(string value)
    {
        this.value = value;
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }

}
