using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FollowSkill : StandardSkill
{
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    float movementDelay = 0;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    float movementRadius = 0;

    private bool canMove = true;
    private bool startCoroutine = true;

    // Update is called once per frame
    public override void doOnUpdate()
    {
        base.doOnUpdate();
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
        if (Vector3.Distance(this.sender.transform.position, this.transform.position) > this.movementRadius)
        {
            if (this.startCoroutine) StartCoroutine(delayCo());
            if (this.canMove) moveTorwardsTarget(this.sender.transform.position);
        }
        else
        {
            this.startCoroutine = true;
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Moving", false);
        }
    }

    private void moveTorwardsTarget(Vector3 position)
    {        
            //Bewegt den Gegner zum Spieler
            Vector3 temp = Vector3.MoveTowards(transform.position, position, this.speed * (Time.deltaTime * this.timeDistortion));
            Vector2 direction = position - this.transform.position;

            this.myRigidbody.MovePosition(temp);
            this.myRigidbody.velocity = Vector2.zero;

            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Moving", true);        
    }
}
