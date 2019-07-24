using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniDialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textfield;

    private float duration;

    private void Start()
    {
        Destroy(this.gameObject, this.duration);
    }

    public void setText(string text)
    {
        this.textfield.text = text;
    }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }
}
