using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : Script
{
    public GameObject impactEffect;
    public LineRenderer lineRenderer;

    [Range(0,5)]
    public float width=0.25f;
    [Range(0, 5)]
    public float animatorWidth = 1;
    [Range(0, Utilities.maxFloatSmall)]
    public float distance = 0;

    private Vector2 direction;

    public override void onDestroy()
    {

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

        this.lineRenderer.startWidth = this.width * this.animatorWidth;
        this.lineRenderer.endWidth = this.width * this.animatorWidth;

        //No hit on itself
        int layerMask = 1 << this.skill.sender.gameObject.layer;
        layerMask = ~layerMask;

        RaycastHit2D hitInfo = Physics2D.Raycast(startpoint, direction, this.distance, layerMask);

        if (hitInfo && !hitInfo.collider.isTrigger)
        {
            Debug.Log(hitInfo.transform.gameObject.name);

            if (Utilities.checkCollision(hitInfo.collider, this.skill))
            {
                Character hittedCharacter = hitInfo.transform.GetComponent<Character>();
                if (hittedCharacter != null) hittedCharacter.gotHit(this.skill);
            }

            if (this.impactEffect != null)
            {
                GameObject fire = Instantiate(this.impactEffect, hitInfo.point, Quaternion.identity);
                fire.hideFlags = HideFlags.HideInHierarchy;
                Destroy(fire, 0.3f);
            }

            this.lineRenderer.SetPosition(0, startpoint);
            this.lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            this.lineRenderer.SetPosition(0, startpoint);
            this.lineRenderer.SetPosition(1, startpoint + direction * this.distance);
        }
    }

}
