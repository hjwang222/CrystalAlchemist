using UnityEngine;
using TMPro;

public class LockOnFrame : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textField;

    public void setValues(string name)
    {
        this.textField.text = name;
    }
}
