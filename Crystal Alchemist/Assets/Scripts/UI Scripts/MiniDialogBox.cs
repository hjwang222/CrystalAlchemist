using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textfield;

    private void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    public void setText(string text)
    {
        this.textfield.text = text;
    }
}
