using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillHoming : SkillExtension
{
    [BoxGroup("Homing-spezifische Attribute")]
    public float offSetTime;

    [BoxGroup("Homing-spezifische Attribute")]
    public float offSetStrength;

    private void Update()
    {
        moveToTarget();
    }   

    private void moveToTarget()
    {
        if (this.skill.target != null)
        {
            this.skill.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.skill.target.transform.position, this.transform.position) > 0.25f)
            {
                //Ermittle Position des Ziels
                Vector2 targetPosition = this.skill.target.transform.position;

                //offSetTime und offSetStrength lassen den Skill nicht direkt, sondern in einer Kurve fliegen
                if (this.offSetTime >= 0)
                {
                    this.offSetTime -= Time.deltaTime;

                    if (Mathf.Abs(this.skill.target.transform.position.x) > Mathf.Abs(this.skill.target.transform.position.y))
                    {
                        targetPosition = new Vector2(targetPosition.x, targetPosition.y + offSetStrength);
                    }
                    else
                    {
                        targetPosition = new Vector2(targetPosition.x + offSetStrength, targetPosition.y);
                    }

                    this.offSetStrength -= (this.offSetStrength / this.offSetTime);
                }

                //Bewege Skill zum Ziel
                Vector3 temp = Vector3.MoveTowards(this.transform.position, targetPosition, this.skill.speed * (Time.deltaTime * this.skill.timeDistortion));

                this.skill.myRigidbody.MovePosition(temp);
                this.skill.myRigidbody.velocity = Vector2.zero;
            }
            else
            {
                //Starte End-Animation wenn der Skill sein Ziel erreicht hat
                CustomUtilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "Explode", true);
            }
        }
        else
        {
            //Zerstöre Skill, wenn das Ziel nicht mehr vorhanden ist. 
            //TODO: Weiter fliegen lassen?
            this.skill.DestroyIt();
        }
    }
}
