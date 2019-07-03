using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChasingEnemy : Enemy
{
    #region Parameter fürs Verfolgen

    [FoldoutGroup("Movement Attributes", expanded: false)]
    public float attackRadius = 0;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private bool backToStart = true;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private List<Transform> path;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private float followPathPause = 0;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private float followPathPrecision = 0.01f;

    [FoldoutGroup("Movement Attributes", expanded: false)]
    [SerializeField]
    private bool followPathInCircle = true;



    private int factor = 1;
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
                if (Vector3.Distance(transform.position, path[currentPoint].position) > followPathPrecision)
                {
                    moveTorwardsTarget(path[currentPoint].position);
                }
                else
                {
                    StartCoroutine(delayMovementCo());
                }
            }
            else if(this.backToStart) moveTorwardsTarget(spawnPosition);

            //Utilities.SetAnimatorParameter(this.animator, "isWakeUp", false);
        }
    }

    private IEnumerator delayMovementCo()
    {
        yield return new WaitForSeconds(this.followPathPause);
        ChangeGoal();
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
        if (currentPoint == path.Count - 1) //letzter Knoten
        {          
            if (this.followPathInCircle)
            {
                this.currentPoint = 0;
            }
            else
            {
                this.factor = -1; //rückwärts laufen
                currentPoint += this.factor;
            }
        }
        else if (currentPoint == 0)//erster Knoten 
        {
            if (!this.followPathInCircle)
            {
                this.factor = 1; //reset to normal
            }
            currentPoint += this.factor;
        }
        else
        {
            currentPoint += this.factor;
        } 
        
        currentGoal = path[currentPoint];
    }

    #endregion

}
