using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : StandardSkill
{
    #region Attributes

    public GameObject impactEffect;   
    [Range(0, Utilities.maxFloatSmall)]
    public float distance = 0;
    private GameObject fire;
    private bool placeFire = true;
    public SpriteRenderer laserSprite;

    #endregion


    #region Overrides

    public override void DestroyIt()
    {        
        this.placeFire = false;        
        this.fire = null;
        base.DestroyIt();
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();
        drawLine(this.rotateIt);
    }

    public override void init()
    {
        base.init();
        drawLine(true);
    }

    #endregion


    #region Functions (private)

    private void drawLine(bool updateRotation)
    {
        //Bestimme Winkel und Position

        float angle;
        Vector2 startpoint;
        Vector3 rotation;

        Utilities.setDirectionAndRotation(this.sender.transform.position, this.sender.direction, this.target,
                                          this.positionOffset, this.positionHeight, this.snapRotationInDegrees, this.rotationModifier,
                                          out angle, out startpoint, out this.direction, out rotation);

        if (this.target != null && updateRotation)
        {
            this.direction = (Vector2)this.target.transform.position - startpoint;
            float temp_angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
            this.direction = Utilities.DegreeToVector2(temp_angle);
        }

        renderLine(startpoint, rotation);
    }

    private void renderLine(Vector2 startpoint, Vector3 rotation)
    {
        //No hit on itself
        int layerMask = 1 << this.sender.gameObject.layer;
        layerMask = ~layerMask;

        RaycastHit2D hitInfo = Physics2D.CircleCast(startpoint, this.laserSprite.size.y / 5, direction, distance, layerMask);

        if ((this.lockOn != null && target != null) || (this.lockOn == null && hitInfo && !hitInfo.collider.isTrigger))
        {
            //Ziel bzw. Collider wurde getroffen, zeichne Linie bis zum Ziel
            Collider2D hitted = hitInfo.collider;
            Vector2 hitpoint = hitInfo.point;

            if (this.target != null)
            {
                //Übernehme Position, wenn ein Ziel vorhanden ist
                hitted = target.GetComponent<Collider2D>();
                hitpoint = target.transform.position;
            }

            if (Utilities.checkCollision(hitted, this))
            {
                Character hittedCharacter = hitted.transform.GetComponent<Character>();
                if (hittedCharacter != null) hittedCharacter.gotHit(this);
            }

            Vector2 temp = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y - startpoint.y) / 2) + startpoint;

            this.laserSprite.transform.position = temp;
            this.laserSprite.size = new Vector2(Vector3.Distance(hitpoint, startpoint), this.laserSprite.size.y);
            this.laserSprite.transform.rotation = Quaternion.Euler(rotation);

            if (this.impactEffect != null)
            {
                //Generiere einen Treffer

                if (this.fire == null && this.placeFire)
                {
                    this.fire = Instantiate(this.impactEffect, hitpoint, Quaternion.identity);
                    this.fire.transform.position = hitpoint;
                    StandardSkill fireSkill = this.fire.GetComponent<StandardSkill>();

                    if (fireSkill != null)
                    {
                        //Position nicht überschreiben
                        fireSkill.setPositionAtStart = false;
                        fireSkill.sender = this.sender;
                    }

                    fire.hideFlags = HideFlags.HideInHierarchy;
                }
                else if (this.fire != null && this.placeFire)
                {
                    this.fire.transform.position = hitpoint;
                }
            }
        }        
        else
        {
            if (this.lockOn == null)
            {
                //Kein Ziel getroffen, zeichne Linie mit max Länge            
                Vector2 temp = new Vector2(this.direction.x * (this.distance / 2), this.direction.y * (this.distance / 2)) + startpoint;

                this.laserSprite.transform.position = temp;
                this.laserSprite.size = new Vector2(this.distance, this.laserSprite.size.y);
                this.laserSprite.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                //Wenn ein Lockon vorhanden aber kein Ziel existiert, kein Laser!
                this.laserSprite.size = new Vector2(0, 0);
            }
        }
    }

    #endregion
}
