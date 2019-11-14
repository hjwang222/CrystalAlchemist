using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LineIndicator : Indicator
{
    [SerializeField]
    private float distance = 10;

    private Vector2 direction;
    private SkillChain chain;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        drawLine(true);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (this.skill != null) drawLine(this.skill.rotateIt());
    }

    private void drawLine(bool updateRotation)
    {
        //Bestimme Winkel und Position

        float snapRotationInDegrees = 0;
        float rotationModifier = 0;
        if (this.skill.GetComponent<SkillRotationModule>() != null)
        {
            snapRotationInDegrees = this.skill.GetComponent<SkillRotationModule>().snapRotationInDegrees;
            rotationModifier = this.skill.GetComponent<SkillRotationModule>().rotationModifier;
        }

        float angle;
        Vector2 startpoint;
        Vector3 rotation;

        Utilities.Rotation.setDirectionAndRotation(this.skill, out angle, out startpoint, out this.direction, out rotation);

        startpoint = this.skill.sender.spriteRenderer.transform.position;

        if (this.skill.target != null && updateRotation)
        {
            this.direction = (Vector2)this.skill.target.transform.position - startpoint;
            float temp_angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
            this.direction = Utilities.Rotation.DegreeToVector2(temp_angle);
        }
        renderLine(startpoint, rotation);
    }

    private void renderLine(Vector2 startpoint, Vector3 rotation)
    {
        //No hit on itself
        int layerMask = 1 << this.skill.sender.gameObject.layer;
        layerMask = ~layerMask;

        //float offset = 0f;
        Vector2 newPosition = startpoint;
        //newPosition = new Vector2(startpoint.x - (this.direction.x * offset), startpoint.y - (this.direction.y * offset));

        RaycastHit2D hitInfo = Physics2D.CircleCast(newPosition, this.indicatorRenderer.size.y + 0.5f, this.direction, distance, layerMask);

        if (hitInfo && !hitInfo.collider.isTrigger)
        {
            //Ziel bzw. Collider wurde getroffen, zeichne Linie bis zum Ziel
            Collider2D hitted = hitInfo.collider;
            Vector2 hitpoint = hitInfo.point;

            if (this.skill.target != null)
            {
                //Übernehme Position, wenn ein Ziel vorhanden ist
                foreach (Collider2D collider in this.skill.target.GetComponentsInChildren<Collider2D>(false))
                {
                    if (!collider.isTrigger) hitted = collider;
                }
                hitpoint = this.skill.target.transform.position;
            }

            Vector2 temp = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

            this.indicatorRenderer.transform.position = temp;
            this.indicatorRenderer.size = new Vector2(Vector3.Distance(hitpoint, startpoint), this.indicatorRenderer.size.y);
            this.indicatorRenderer.transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            //Kein Ziel getroffen, zeichne Linie mit max Länge            
            Vector2 temp = new Vector2(this.direction.x * (this.distance / 2), this.direction.y * (this.distance / 2)) + startpoint;

            this.indicatorRenderer.transform.position = temp;
            this.indicatorRenderer.size = new Vector2(this.distance, this.indicatorRenderer.size.y);
            this.indicatorRenderer.transform.rotation = Quaternion.Euler(rotation);
        }
    }

}
