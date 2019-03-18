using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangScript : Script
{
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
        //got Hit -> Back to Target
        if (skill.sender != null
            && hittedCharacter.tag != skill.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Object"))
        {
            this.skill.scriptActivationTimeLeft = 0;
        }

        if (hittedCharacter.CompareTag("Item"))
        {
            this.skill.scriptActivationTimeLeft = 0;
            hittedCharacter.transform.position = this.skill.transform.position;
        }
    }

    public override void onEnter(Collider2D hittedCharacter)
    {
        
    }

    private void moveToTarget()
    {
        if (this.skill.sender != null)
        {
            skill.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.skill.sender.transform.position, this.skill.transform.position) > 0.25f)
            {
                Vector3 temp = Vector3.MoveTowards(skill.transform.position, this.skill.sender.transform.position, skill.speed * (Time.deltaTime * skill.timeDistortion));

                skill.myRigidbody.MovePosition(temp);
                skill.myRigidbody.velocity = Vector2.zero;
            }
            else
            {
                skill.DestroyIt();
            }
        }
    }
}
