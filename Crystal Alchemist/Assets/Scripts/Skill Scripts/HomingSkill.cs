using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSkill : StandardSkill
{
    #region Attributes
    public float offSetTime;
    public float offSetStrength;
    #endregion


    #region Overrides
    public override void doOnUpdate()
    {
        base.doOnUpdate();
        moveToTarget();
    }
    #endregion


    #region Functions (private)

    private void moveToTarget()
    {
        if (this.target != null)
        {                        
            this.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.target.transform.position, this.transform.position) > 0.25f)
            {
                //Ermittle Position des Ziels
                Vector2 targetPosition = this.target.transform.position;                

                //offSetTime und offSetStrength lassen den Skill nicht direkt, sondern in einer Kurve fliegen
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

                //Bewege Skill zum Ziel
                Vector3 temp = Vector3.MoveTowards(this.transform.position, targetPosition, this.speed * (Time.deltaTime * this.timeDistortion));

                this.myRigidbody.MovePosition(temp);
                this.myRigidbody.velocity = Vector2.zero;
            }
            else
            {                
                //Starte End-Animation wenn der Skill sein Ziel erreicht hat
                if (this.animator != null) this.animator.SetBool("Explode", true);                
            }
        }
        else
        {
            //Zerstöre Skill, wenn das Ziel nicht mehr vorhanden ist. 
            //TODO: Weiter fliegen lassen?
            this.DestroyIt();
        }
    }

    #endregion
}
