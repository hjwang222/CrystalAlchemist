using Sirenix.OdinInspector;
using UnityEngine;

public class BeamIndicator : GroundIndicator
{
    [BoxGroup("Main")]
    [SerializeField]
    [Required]
    private float distance = 1f;

    public override void SetIndicator()
    {
        SetOuter();
    }

    public override void SetOuter()
    {
        base.SetOuter();
        this.outline.SetLine(this.distance);
    }
}
