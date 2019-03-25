using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : StandardSkill
{
    public GameObject impactEffect;   
    [Range(0, Utilities.maxFloatSmall)]
    public float distance = 0;
    private GameObject fire;
    private bool placeFire = true;
    public SpriteRenderer laserSprite;

    public new void DestroyIt()
    {        
        this.placeFire = false;        
        this.fire = null;
        base.DestroyIt();
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();
        renderLine(this.rotateIt);
    }

    public override void init()
    {
        base.init();
        renderLine(true);
    }

    private Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    private Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    private void renderLine(bool updateRotation)
    {
        //TODO: AUSLAGERN!
        if (updateRotation)
        {
            this.direction = this.sender.direction;
        }

        Vector2 startpoint = new Vector2(this.sender.transform.position.x + (this.direction.x * this.positionOffset),
                                         this.sender.transform.position.y + (this.direction.y * this.positionOffset) + this.positionHeight);

        if (this.target != null && updateRotation)
        {
            this.direction = (Vector2)this.target.transform.position - startpoint;
            float temp_angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
            this.direction = DegreeToVector2(temp_angle);
        }
        
        //Utilities.berechneWinkel(this.sender.transform.position, this.sender.direction, this.positionOffset, this.rotateIt, this.snapRotationInDegrees, out angle, out start, out this.direction);

        float angle = 0;
        angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;

        if (this.snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / this.snapRotationInDegrees) * this.snapRotationInDegrees;
            this.direction = DegreeToVector2(angle);
        }

        Vector3 rotation = new Vector3(0, 0, angle);

        //No hit on itself
        int layerMask = 1 << this.sender.gameObject.layer;
        layerMask = ~layerMask;

        RaycastHit2D hitInfo = Physics2D.CircleCast(startpoint, this.laserSprite.size.y/5, direction, distance, layerMask);

        if ((hitInfo && !hitInfo.collider.isTrigger) || target != null)
        {
            Collider2D hitted = hitInfo.collider;
            Vector2 hitpoint = hitInfo.point;

            if(this.target != null)
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

            Vector2 temp = new Vector2((hitpoint.x - startpoint.x) / 2, (hitpoint.y- startpoint.y) / 2)+ startpoint;
            
            this.laserSprite.transform.position = temp;
            this.laserSprite.size = new Vector2(Vector3.Distance(hitpoint, startpoint), this.laserSprite.size.y);
            this.laserSprite.transform.rotation = Quaternion.Euler(rotation);

            if (this.impactEffect != null)
            {
                if (this.fire == null && this.placeFire)
                {
                    this.fire = Instantiate(this.impactEffect, hitpoint, Quaternion.identity);
                    StandardSkill fireSkill = this.fire.GetComponent<StandardSkill>();

                    if (fireSkill != null)
                    {
                        fireSkill.sender = this.sender;
                    }

                    fire.hideFlags = HideFlags.HideInHierarchy;
                    //Destroy(fire, 0.5f);
                }
                else if (this.fire != null && this.placeFire)
                {
                    this.fire.transform.position = hitpoint;
                }
            }

            //HIT
            //1. Height anpassen 
            //2. Position anpassen
        }
        else
        {
            //NO HIT
            Vector2 temp = new Vector2(this.direction.x * (this.distance / 2), this.direction.y * (this.distance / 2)) + startpoint;

            this.laserSprite.transform.position = temp;
            this.laserSprite.size = new Vector2(this.distance, this.laserSprite.size.y);
            this.laserSprite.transform.rotation = Quaternion.Euler(rotation);
        }
    }

}
