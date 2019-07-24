using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public StandardSkill skill;
    public float distance = 10;
    public SpriteRenderer laserSprite;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        drawLine(true);
    }

    // Update is called once per frame
    void Update()
    {
        drawLine(this.skill.rotateIt);
    }



    private void drawLine(bool updateRotation)
    {
        //Bestimme Winkel und Position

        float angle;
        Vector2 startpoint;
        Vector3 rotation;
        

        Utilities.Rotation.setDirectionAndRotation(this.skill.sender, this.skill.target,
                                          0, 0, this.skill.snapRotationInDegrees, this.skill.rotationModifier,
                                          out angle, out startpoint, out this.direction, out rotation);

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

        float offset = 1f;
        Vector2 newPosition = new Vector2(startpoint.x - (this.direction.x * offset), startpoint.y - (this.direction.y * offset));

        RaycastHit2D hitInfo = Physics2D.CircleCast(newPosition, this.laserSprite.size.y, this.direction, distance, layerMask);

        if (hitInfo && !hitInfo.collider.isTrigger)
        {
            //Ziel bzw. Collider wurde getroffen, zeichne Linie bis zum Ziel
            Collider2D hitted = hitInfo.collider;
            Vector2 hitpoint = hitInfo.point;

            if (this.skill.target != null)
            {
                //Übernehme Position, wenn ein Ziel vorhanden ist
                hitted = this.skill.target.GetComponent<Collider2D>();
                hitpoint = this.skill.target.transform.position;
            }

            Vector2 temp = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

            this.laserSprite.transform.position = temp;
            this.laserSprite.size = new Vector2(Vector3.Distance(hitpoint, startpoint), this.laserSprite.size.y);
            this.laserSprite.transform.rotation = Quaternion.Euler(rotation);            
        }
        else
        {
            
                //Kein Ziel getroffen, zeichne Linie mit max Länge            
                Vector2 temp = new Vector2(this.direction.x * (this.distance / 2), this.direction.y * (this.distance / 2)) + startpoint;

                this.laserSprite.transform.position = temp;
                this.laserSprite.size = new Vector2(this.distance, this.laserSprite.size.y);
                this.laserSprite.transform.rotation = Quaternion.Euler(rotation);
            
            
        }
    }

}
