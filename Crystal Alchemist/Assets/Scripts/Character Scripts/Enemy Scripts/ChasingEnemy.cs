using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : Enemy
{
    #region Parameter fürs Verfolgen

    [Header("Movement-Parameter")]
    public float chaseRadius;
    public float attackRadius;

    #endregion


    #region Update und Movement Funktionen
    void Update()
    {
        //Ziel auswählen und verfolgen
        //resetVelocity();
        
        regeneration();
        setTarget(); //TODO
        moveCharacter();

    }

    public void moveCharacter()
    {
        if (currentState == CharacterState.dead)
        {            
            //STOP ANIMATION
            return;
        }

        if (this.target != null)
        {
            //Wenn der Spieler innerhalb vom Chase-Radius ist und auch nur solange bis der Gegner den Spieler nicht berührt!
            if (Vector3.Distance(this.target.transform.position, transform.position) <= this.chaseRadius
                && Vector3.Distance(this.target.transform.position, transform.position) > this.attackRadius)
            {
                if (this.currentState == CharacterState.idle || this.currentState == CharacterState.walk && this.currentState != CharacterState.knockedback)
                {
                    //Bewegt den Gegner zum Spieler
                    Vector3 temp = Vector3.MoveTowards(transform.position, this.target.transform.position, this.speed * (Time.deltaTime * this.timeDistortion));
                    
                    changeAnim(temp - transform.position);
                    this.myRigidbody.MovePosition(temp);
                    this.myRigidbody.velocity = Vector2.zero;

                    changeState(CharacterState.walk); //Gegner bewegt sich gerade

                    Utilities.SetAnimatorParameter(this.animator, "isWakeUp", true);
                }
            }
            else if (Vector3.Distance(this.target.transform.position, transform.position) > this.chaseRadius)
            {
                Utilities.SetAnimatorParameter(this.animator, "isWakeUp", false);
            }
        }
    }

    #endregion

}
