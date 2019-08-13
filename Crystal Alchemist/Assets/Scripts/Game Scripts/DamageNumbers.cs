using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    [HideInInspector]
    public float number;

    private Color[] color;

    [SerializeField]
    private float factor = 1;

    public void setcolor(Color[] color)
    {
        this.color = color;
    }

    public void Start()
    {
        string text = "";
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();

        number *= factor;

        if (number != 0)
        {
            if(number > 0) text = "+" + Utilities.Format.setDamageNumber(number, 1f);
            else text = Utilities.Format.setDamageNumber(number, 1f);

            textmeshPro.color = this.color[0];
            textmeshPro.outlineColor = this.color[1];
            textmeshPro.outlineWidth = 0.25f;
        }        
        else
        {
            text = "";
            textmeshPro.color = new Color32(255, 255, 255, 255);
            textmeshPro.outlineColor = new Color32(125, 125, 125, 255);
            textmeshPro.outlineWidth = 0.25f;
        }

        textmeshPro.text = text;

        Destroy(this.gameObject, 2f);
    }

}
