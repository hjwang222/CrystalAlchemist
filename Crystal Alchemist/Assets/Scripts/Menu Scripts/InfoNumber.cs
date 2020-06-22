using UnityEngine;
using TMPro;

public class InfoNumber : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI numberField;

    public void SetValue(int value)
    {
        this.numberField.text = value+"";
        if (value > 99) this.numberField.text = "99+";
    }
}
