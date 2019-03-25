using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSkill : StandardSkill
{
    [Tooltip("Zeitpunkt der Scriptaktivierung")]
    [Range(0, Utilities.maxFloatSmall)]
    public float timeToMoveBack = 0;
    private float durationThenBackToSender = 0;

    public override void init()
    {
        base.init();
        this.durationThenBackToSender = timeToMoveBack;
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();

        if (this.durationThenBackToSender > 0)
        {
            this.durationThenBackToSender -= (Time.deltaTime * this.timeDistortion);
        }
        else
        {
            moveToTarget();
        }        
    }

    public override void OnTriggerStay2D (Collider2D hittedCharacter)
    {
        base.OnTriggerStay2D(hittedCharacter);
        //got Hit -> Back to Target
        if (this.sender != null
            && hittedCharacter.tag != this.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Object"))
        {
            this.durationThenBackToSender = 0;
        }

        if (hittedCharacter.CompareTag("Item"))
        {
            this.durationThenBackToSender = 0;
            hittedCharacter.transform.position = this.transform.position;
        }
    }

    private void moveToTarget()
    {
        if (this.sender != null)
        {
            this.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.sender.transform.position, this.transform.position) > 0.25f)
            {
                Vector3 temp = Vector3.MoveTowards(this.transform.position, this.sender.transform.position, this.speed * (Time.deltaTime * this.timeDistortion));

                this.myRigidbody.MovePosition(temp);
                this.myRigidbody.velocity = Vector2.zero;
            }
            else
            {
                this.DestroyIt();
            }
        }
    }
}
