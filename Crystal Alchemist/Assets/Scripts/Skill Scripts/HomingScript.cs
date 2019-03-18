using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingScript : Script
{
    public float offSetTime;
    public float offSetStrength;

    public override void onDestroy()
    {

    }

    public override void onUpdate()
    {
        moveToTarget();
    }

    public override void onInitialize()
    {

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

    private void moveToTarget()
    {
        if (this.skill.target != null)
        {            
            skill.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.skill.target.transform.position, this.skill.transform.position) > 0.25f)
            {
                Vector2 targetPosition = this.skill.target.transform.position;                

                if (this.offSetTime >= 0)
                {
                    this.offSetTime -= Time.deltaTime;

                    if(Mathf.Abs(this.skill.target.transform.position.x) > Mathf.Abs(this.skill.target.transform.position.y))
                    {
                        targetPosition = new Vector2(targetPosition.x, targetPosition.y+offSetStrength);
                    }
                    else
                    {
                        targetPosition = new Vector2(targetPosition.x + offSetStrength, targetPosition.y);
                    }

                    this.offSetStrength -= (this.offSetStrength / this.offSetTime);
                }

                Vector3 temp = Vector3.MoveTowards(skill.transform.position, targetPosition, skill.speed * (Time.deltaTime * skill.timeDistortion));

                skill.myRigidbody.MovePosition(temp);
                skill.myRigidbody.velocity = Vector2.zero;
            }
            else
            {
                //this.skill.landAttack(this.skill.target);
                if (this.skill.animator != null) skill.animator.SetBool("Explode", true);
                //else skill.DestroyIt();
            }
        }
        else
        {
            this.skill.DestroyIt();
        }
    }
}
