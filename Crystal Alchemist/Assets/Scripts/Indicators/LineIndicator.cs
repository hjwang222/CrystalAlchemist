using UnityEngine;

public class LineIndicator : Indicator
{
    [SerializeField]
    private float distance = 100f;

    public override void Start()
    {
        base.Start();
        this.GetSpriteRenderer().enabled = false;
    }

    public override void Update()
    {
        LineRenderUtil.RenderLine(this.GetSender(), this.GetTarget(), this.distance, this.GetSpriteRenderer(), null, out Collider2D hitted, out Vector2 hitPoint);            
    }

}
