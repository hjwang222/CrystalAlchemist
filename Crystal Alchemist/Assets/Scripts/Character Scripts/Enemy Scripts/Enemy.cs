using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [HideInInspector]
    public Character target;


    #region Start

    private void Start()
    {
        init();
        setTarget();
        //this.target = GameObject.FindWithTag("Player").transform;   //Position und Size
        Utilities.SetAnimatorParameter(this.animator, "isWakeUp", true);
    }

    #endregion


    private void Update()
    {
        setTarget(); //TODO Aggro System
    }



    #region KI (Aggro, Animation, StateMachine)

    public void setTarget()
    {
        //Aggro-Verhalten
        //TODO: Finde den nähsten Spieler
        //TODO: Wechsel erst nach x Sekunden oder wenn außer Reichweite
        if (this.target == null)
        {
            Character temp = GameObject.FindWithTag("Player").GetComponent<Character>();

            if (temp.currentState != CharacterState.inDialog)
            {
                this.target = temp;
            }
            else
            {
                this.target = null;
            }
        }
    }

    private void setAnimFloat(Vector2 setVector)
    {
        this.direction = setVector;

        Utilities.SetAnimatorParameter(this.animator, "moveX", setVector.x);
        Utilities.SetAnimatorParameter(this.animator, "moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction)
    {        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) setAnimFloat(Vector2.right);
            else if (direction.x < 0) setAnimFloat(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0) setAnimFloat(Vector2.up);
            else if (direction.y < 0) setAnimFloat(Vector2.down);
        }
    }

    public void changeState(CharacterState newState)
    {
        if (this.currentState != newState) this.currentState = newState;
    }

    #endregion

}
