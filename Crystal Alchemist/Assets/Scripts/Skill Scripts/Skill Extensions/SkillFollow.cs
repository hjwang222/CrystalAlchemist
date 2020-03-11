using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillFollow : SkillProjectile
{
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    float movementDelay = 0;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    float movementRadius = 0;

    private bool canMove = true;
    private bool startCoroutine = true;

    private void Update()
    {
        moveIt();
    }

    private IEnumerator delayCo()
    {
        this.startCoroutine = false;
        this.canMove = false;
        yield return new WaitForSeconds(this.movementDelay);
        this.canMove = true;
    }

    private void moveIt()
    {
        if (Vector3.Distance(this.skill.sender.transform.position, this.transform.position) > this.movementRadius)
        {
            if (this.startCoroutine) StartCoroutine(delayCo());
            if (this.canMove) moveTorwardsTarget(this.skill.sender.transform.position);
        }
        else
        {
            this.startCoroutine = true;
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "Moving", false);
        }
    }

    private void moveTorwardsTarget(Vector3 position)
    {
        //Bewegt den Gegner zum Spieler
        Vector3 temp = Vector3.MoveTowards(transform.position, position, this.speed * (Time.deltaTime * this.skill.getTimeDistortion()));
        Vector2 direction = position - this.transform.position;

        this.skill.myRigidbody.MovePosition(temp);
        this.stopVelocity();

        CustomUtilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "Moving", true);
    }
}
