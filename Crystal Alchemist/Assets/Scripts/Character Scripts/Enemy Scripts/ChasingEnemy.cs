using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChasingEnemy : Enemy
{
    #region Parameter fürs Verfolgen

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    public float attackRadius = 0;

    [FoldoutGroup("Enemy Attributes", expanded: false)]
    [SerializeField]
    private bool backToStart = true;

    [FoldoutGroup("Patrol Attributes", expanded: false)]
    [SerializeField]
    private List<Transform> path;
    private int currentPoint = 0;
    private Transform currentGoal;

    #endregion


    #region Update und Movement Funktionen
    private new void Update()
    {
        base.Update();
        regeneration();
        moveCharacter();
    }

    private void moveCharacter()
    {
        if (currentState == CharacterState.dead)
        {
            //STOP ANIMATION
            return;
        }

        if (this.target != null)
        {
            //Wenn der Spieler innerhalb vom Chase-Radius ist und auch nur solange bis der Gegner den Spieler nicht berührt!
            if (Vector3.Distance(this.target.transform.position, transform.position) > this.attackRadius)
            {
                moveTorwardsTarget(this.target.transform.position);
            }
        }
        else
        {
            if(this.path.Count > 0)
            {
                if (Vector3.Distance(transform.position, path[currentPoint].position) > float.Epsilon)
                {
                    moveTorwardsTarget(path[currentPoint].position);
                }
                else
                {
                    ChangeGoal();
                }
            }
            else if(this.backToStart) moveTorwardsTarget(spawnPosition);

            //Utilities.SetAnimatorParameter(this.animator, "isWakeUp", false);
        }
    }

    private void moveTorwardsTarget(Vector3 position)
    {
        if (this.currentState == CharacterState.idle || this.currentState == CharacterState.walk && this.currentState != CharacterState.knockedback)
        {
            //Bewegt den Gegner zum Spieler
            Vector3 temp = Vector3.MoveTowards(transform.position, position, this.speed * (Time.deltaTime * this.timeDistortion));

            changeAnim(temp - transform.position);
            this.myRigidbody.MovePosition(temp);
            this.myRigidbody.velocity = Vector2.zero;

            changeState(CharacterState.walk); //Gegner bewegt sich gerade

            Utilities.SetAnimatorParameter(this.animator, "isWakeUp", true);
        }
    }


    private void ChangeGoal()
    {
        if (currentPoint == path.Count - 1)
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }

    #endregion

}
