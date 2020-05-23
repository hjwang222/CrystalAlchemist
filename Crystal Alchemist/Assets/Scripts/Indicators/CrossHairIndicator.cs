using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class CrossHairIndicator : Indicator
{
    [SerializeField]
    [Required]
    private TextMeshPro textField;

    public override void Initialize(Character sender, Character target)
    {
        base.Initialize(sender, target);

        this.transform.position = target.transform.position;
        this.transform.rotation = Quaternion.identity;
        this.transform.SetParent(target.transform);

        this.textField.text = target.GetCharacterName();
    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        this.textField.color = color;
    }
}
