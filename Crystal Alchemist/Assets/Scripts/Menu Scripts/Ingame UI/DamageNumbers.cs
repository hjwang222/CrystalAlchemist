using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField]
    private float factor = 1;

    [SerializeField]
    private TextMeshPro textField;

    public void Initialize(float value, Color[] color)
    {
        float number = value * factor;
        string text = "";

        if (number != 0)
        {
            
            if (number > 0) text = "+" + FormatUtil.formatFloatToString(number, 1f);
            else text = FormatUtil.formatFloatToString(number, 1f);

            this.textField.color = color[0];
            this.textField.outlineColor = color[1];
            this.textField.outlineWidth = 0.25f;            
        }        
        else
        {
            text = "";
            this.textField.color = new Color32(255, 255, 255, 255);
            this.textField.outlineColor = new Color32(125, 125, 125, 255);
            this.textField.outlineWidth = 0.25f;
        }

        this.textField.text = text;
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);       
    }

}
