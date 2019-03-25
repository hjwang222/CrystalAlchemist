using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSkill : StandardSkill
{
    public float offSetTime;
    public float offSetStrength;

    public override void doOnUpdate()
    {
        base.doOnUpdate();
        moveToTarget();
    }

    private void moveToTarget()
    {
        if (this.target != null)
        {            
            this.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.target.transform.position, this.transform.position) > 0.25f)
            {
                Vector2 targetPosition = this.target.transform.position;                

                if (this.offSetTime >= 0)
                {
                    this.offSetTime -= Time.deltaTime;

                    if(Mathf.Abs(this.target.transform.position.x) > Mathf.Abs(this.target.transform.position.y))
                    { 
                        targetPosition = new Vector2(targetPosition.x, targetPosition.y+offSetStrength);
                    }
                    else
                    {
                        targetPosition = new Vector2(targetPosition.x + offSetStrength, targetPosition.y);
                    }

                    this.offSetStrength -= (this.offSetStrength / this.offSetTime);
                }

                Vector3 temp = Vector3.MoveTowards(this.transform.position, targetPosition, this.speed * (Time.deltaTime * this.timeDistortion));

                this.myRigidbody.MovePosition(temp);
                this.myRigidbody.velocity = Vector2.zero;
            }
            else
            {
                //this.skill.landAttack(this.skill.target);
                if (this.animator != null) this.animator.SetBool("Explode", true);
                //else skill.DestroyIt();
            }
        }
        else
        {
            this.DestroyIt();
        }
    }
}
