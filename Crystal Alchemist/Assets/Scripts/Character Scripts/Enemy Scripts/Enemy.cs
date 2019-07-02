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
        Utilities.SetAnimatorParameter(this.animator, "isWakeUp", true);
    }

    #endregion






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            setTarget(collision.GetComponent<Character>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            setTarget(null);
        }
    }



    #region KI (Aggro, Animation, StateMachine)

    public void setTarget(Character target)
    {
        if (this.target != target)
        {            
            this.target = target;
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
