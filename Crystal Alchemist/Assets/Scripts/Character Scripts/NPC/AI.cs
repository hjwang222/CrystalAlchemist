using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AI : Character
{
    [Required]
    [BoxGroup("Pflichtfelder")]
    public GameObject dialogPosition;

    [HideInInspector]
    public Character target;

    [HideInInspector]
    public Character partner;

    private void Start()
    {
        init();

        this.target = null;
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isWakeUp", true);
    }


    #region Animation, StateMachine


    private void setAnimFloat(Vector2 setVector)
    {
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", setVector.x);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction)
    {
        //TODO: To be tested
        this.direction = direction;

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
