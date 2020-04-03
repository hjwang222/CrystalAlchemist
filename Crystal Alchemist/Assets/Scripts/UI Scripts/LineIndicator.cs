using UnityEngine;

public class LineIndicator : Indicator
{
    public override void Update()
    {
        Vector2 startpoint = this.GetSender().transform.position;
        Vector2 hitpoint = this.GetTarget().transform.position;
        Vector2 direction = (hitpoint - startpoint).normalized;

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Vector3 rotation = new Vector3(0, 0, angle);
        Vector2 position = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

        setLaser(position, Vector3.Distance(hitpoint, startpoint), rotation);
    }

    private void setLaser(Vector2 position, float distance, Vector3 rotation)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.transform.position = position;
        spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
        spriteRenderer.transform.rotation = Quaternion.Euler(rotation);        
    }
}
