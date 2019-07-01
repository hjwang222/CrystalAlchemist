using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateLocation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;
    
    public void updateLocationText(string text)
    {
        this.textField.text = text;
    }

}
