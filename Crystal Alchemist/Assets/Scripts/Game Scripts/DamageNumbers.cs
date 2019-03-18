using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public float number;

    public void Start()
    {
        string text = "";
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();



        if (number > 0)
        {
            text = "+" + Utilities.setDamageNumber(number * 4,1f);
            textmeshPro.color = new Color32(0, 255, 0, 255);
            textmeshPro.outlineColor = new Color32(0, 128, 0, 255);
            textmeshPro.outlineWidth = 0.25f;
        }
        else if(number < 0)
        {
            text = Utilities.setDamageNumber(number * 4, 1f);
            textmeshPro.color = new Color32(255, 0, 0, 255);
            textmeshPro.outlineColor = new Color32(125, 0, 0, 255);
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
