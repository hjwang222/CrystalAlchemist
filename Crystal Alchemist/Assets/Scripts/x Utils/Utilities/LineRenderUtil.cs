using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LineRenderUtil
{
    public static void RenderLine(Character sender, Character target, float distance, SpriteRenderer spriteRenderer, Light2D lights,
                             out Collider2D hitted, out Vector2 hitPoint)
    {
        spriteRenderer.enabled = true;
        hitted = null;
        hitPoint = Vector2.zero;
        Vector2 startpoint = sender.GetShootingPosition();

        if (target != null)
        {
            //draw Laser to target
            hitPoint = target.GetShootingPosition();
            hitted = getCollider(target);
            drawLaserToTarget(startpoint, hitPoint, spriteRenderer, lights);
        }
        else
        {
            //draw normal Laser
            Vector2 direction = sender.direction;
            RaycastHit2D hitInfo = new RaycastHit2D();

            if (sender != null)
            {
                int layerMask = 1 << sender.gameObject.layer;
                layerMask = ~layerMask;
                hitInfo = Physics2D.CircleCast(startpoint, spriteRenderer.size.y / 5, direction, distance, layerMask);
            }
            else
            {
                hitInfo = Physics2D.CircleCast(startpoint, spriteRenderer.size.y / 5, direction, distance);
            }

            hitted = hitInfo.collider;
            hitPoint = hitInfo.point;

            if (hitInfo && !hitInfo.collider.isTrigger)
            {
                drawLaserToTarget(startpoint, hitPoint, spriteRenderer, lights);
            }
            else
            {
                drawLongLaser(startpoint, direction, distance, spriteRenderer, lights);
            }
        }
    }

    private static Collider2D getCollider(Character character)
    {
        //Übernehme Position, wenn ein Ziel vorhanden ist
        foreach (Collider2D collider in character.GetComponentsInChildren<Collider2D>(false))
        {
            if (!collider.isTrigger) return collider;
        }

        return null;
    }

    private static void drawLaserToTarget(Vector2 startpoint, Vector2 hitpoint, SpriteRenderer spriteRenderer, Light2D lights)
    {
        Vector2 direction = (hitpoint - startpoint).normalized;
        Vector2 position = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

        setLaserSprite(position, Vector3.Distance(hitpoint, startpoint), GetRotation(direction), spriteRenderer, lights);
    }

    private static void drawLongLaser(Vector2 startpoint, Vector2 direction, float distance, SpriteRenderer spriteRenderer, Light2D lights)
    {
        Vector2 position = new Vector2(direction.x * (distance / 2), direction.y * (distance / 2)) + startpoint;

        setLaserSprite(position, distance, GetRotation(direction), spriteRenderer, lights);
    }

    private static void setLaserSprite(Vector2 position, float distance, Vector3 rotation, SpriteRenderer spriteRenderer, Light2D lights)
    {
        spriteRenderer.transform.position = position;
        spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
        spriteRenderer.transform.rotation = Quaternion.Euler(rotation);

        if (lights != null) lights.transform.localScale = new Vector2(distance, lights.transform.localScale.y);
    }

    public static void Renderempty(SpriteRenderer spriteRenderer, Light2D lights)
    {
        spriteRenderer.size = new Vector2(0, 0);
        if (lights != null) lights.transform.localScale = new Vector2(0, 0);
    }

    private static Vector3 GetRotation(Vector2 direction)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        return new Vector3(0, 0, angle);
    }
}
