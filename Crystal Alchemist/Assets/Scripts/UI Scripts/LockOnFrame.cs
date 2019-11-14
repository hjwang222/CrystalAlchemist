using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockOnFrame : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    public void setValues(string name)
    {
        this.textField.text = name;
    }


}
