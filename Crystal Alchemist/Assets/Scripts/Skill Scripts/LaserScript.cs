using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : Script
{
    public GameObject impactEffect;   
    [Range(0, Utilities.maxFloatSmall)]
    public float distance = 0;
    private Vector2 direction;
    public SpriteRenderer spriteRenderer;
    private GameObject fire;
    private bool placeFire = true;

    public override void onDestroy()
    {
        this.placeFire = false;        
        this.fire = null;
    }

    public override void onUpdate()
    {
        renderLine(skill.rotateIt);
    }

    public override void onInitialize()
    {
        renderLine(true);
    }

    public override void onExit(Collider2D hittedCharacter)
    {

    }

    public override void onStay(Collider2D hittedCharacter)
    {

    }

    public override void onEnter(Collider2D hittedCharacter)
    {


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
            this.direction = this.skill.sender.direction;
        }

        Vector2 startpoint = new Vector2(this.skill.sender.transform.position.x + (this.direction.x * this.skill.positionOffset),
                                         this.skill.sender.transform.position.y + (this.direction.y * this.skill.positionOffset) + this.skill.positionHeight);

        if (this.skill.target != null && updateRotation)
        {
            this.direction = (Vector2)this.skill.target.transform.position - startpoint;
            float temp_angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
            this.direction = DegreeToVector2(temp_angle);
        }
        
        //Utilities.berechneWinkel(this.sender.transform.position, this.sender.direction, this.positionOffset, this.rotateIt, this.snapRotationInDegrees, out angle, out start, out this.direction);

        float angle = 0;
        angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;

        if (this.skill.snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / this.skill.snapRotationInDegrees) * this.skill.snapRotationInDegrees;
            this.direction = DegreeToVector2(angle);
        }

        Vector3 rotation = new Vector3(0, 0, angle);

        //No hit on itself
        int layerMask = 1 << this.skill.sender.gameObject.layer;
        layerMask = ~layerMask;

        RaycastHit2D hitInfo = Physics2D.Raycast(startpoint, direction, this.distance, layerMask);

        if (hitInfo && !hitInfo.collider.isTrigger)
        {
            

            if (Utilities.checkCollision(hitInfo.collider, this.skill))
            {
                Character hittedCharacter = hitInfo.transform.GetComponent<Character>();
                if (hittedCharacter != null) hittedCharacter.gotHit(this.skill);
            }

            if (this.impactEffect != null)
            {
                if (this.fire == null && this.placeFire)
                {
                    this.fire = Instantiate(this.impactEffect, hitInfo.point, Quaternion.identity);
                    Skill fireSkill = this.fire.GetComponent<Skill>();

                    if(fireSkill != null)
                    {
                        fireSkill.sender = this.skill.sender;
                    }

                    fire.hideFlags = HideFlags.HideInHierarchy;
                    //Destroy(fire, 0.5f);
                }
                else if(this.fire != null && this.placeFire)
                {
                    this.fire.transform.position = hitInfo.point;
                }
            }

            Vector2 temp = new Vector2((hitInfo.point.x - startpoint.x) / 2, (hitInfo.point.y- startpoint.y) / 2)+ startpoint;
            
            this.spriteRenderer.transform.position = temp;
            this.spriteRenderer.size = new Vector2(Vector3.Distance(hitInfo.point, startpoint), this.spriteRenderer.size.y);
            this.spriteRenderer.transform.rotation = Quaternion.Euler(rotation);
            //HIT
            //1. Height anpassen 
            //2. Position anpassen
        }
        else
        {
            //NO HIT
            
            this.spriteRenderer.transform.position = startpoint + direction * this.distance;
            this.spriteRenderer.size = new Vector2( this.distance, this.spriteRenderer.size.y);
            this.spriteRenderer.transform.rotation = Quaternion.Euler(rotation);
        }
    }

}
