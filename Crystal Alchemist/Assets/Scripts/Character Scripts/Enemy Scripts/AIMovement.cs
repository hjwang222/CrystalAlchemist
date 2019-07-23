using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIMovement : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private Enemy enemy;


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

    private bool standStill = false;
    private int factor = 1;
    private int currentPoint = 0;
    private Transform currentGoal;

    #endregion


    #region Update und Movement Funktionen
    private void Update()
    {     
        if(this.enemy.currentState != CharacterState.dead
        && this.enemy.currentState != CharacterState.manually) moveCharacter();
    }

    private void moveCharacter()
    {     
        if (this.enemy.target != null)
        {
            //Wenn der Spieler innerhalb vom Chase-Radius ist und auch nur solange bis der Gegner den Spieler nicht berührt!
            if (Vector3.Distance(this.enemy.target.transform.position, this.transform.position) > this.attackRadius)
            {
                moveTorwardsTarget(this.enemy.target.transform.position);
            }
        }
        else
        {
            if (!this.standStill)
            {
                if (this.path.Count > 0)
                {
                    if (Vector3.Distance(transform.position, path[currentPoint].position) > followPathPrecision)
                    {
                        moveTorwardsTarget(path[currentPoint].position);
                    }
                    else
                    {
                        ChangeGoal();
                        StartCoroutine(delayMovementCo());
                    }
                }
                else if (this.backToStart) moveTorwardsTarget(this.enemy.spawnPosition);
            }
            //Utilities.SetAnimatorParameter(this.animator, "isWakeUp", false);
        }
    }

    private IEnumerator delayMovementCo()
    {
        this.standStill = true;
        yield return new WaitForSeconds(this.followPathPause);
        this.standStill = false;
    }

    private void moveTorwardsTarget(Vector3 position)
    {
        if (this.enemy.currentState == CharacterState.idle || this.enemy.currentState == CharacterState.walk && this.enemy.currentState != CharacterState.knockedback)
        {
            //Bewegt den Gegner zum Spieler
            Vector3 temp = Vector3.MoveTowards(transform.position, position, this.enemy.speed * (Time.deltaTime * this.enemy.timeDistortion));

            this.enemy.changeAnim(temp - transform.position);

            this.enemy.myRigidbody.MovePosition(temp);
            this.enemy.myRigidbody.velocity = Vector2.zero;

            //changeState(CharacterState.walk); //Gegner bewegt sich gerade

            Utilities.UnityUtils.SetAnimatorParameter(this.enemy.animator, "isWakeUp", true);
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
