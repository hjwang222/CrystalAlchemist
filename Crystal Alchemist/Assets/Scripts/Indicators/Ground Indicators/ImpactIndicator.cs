using Sirenix.OdinInspector;
using UnityEngine;

public class ImpactIndicator : GroundIndicator
{
    [BoxGroup("Main")]
    [SerializeField]
    [Required]
    private new Collider2D collider;

    public override void SetIndicator()
    {
        SetOuter();
    }

    public override void SetOuter()
    {
        base.SetOuter();
        this.outline.SetCollider(this.collider);
    }
}
