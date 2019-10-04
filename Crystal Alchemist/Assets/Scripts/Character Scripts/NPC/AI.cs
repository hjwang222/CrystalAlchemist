using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AI : Character
{
    [Required]
    [BoxGroup("Easy Access")]
    public GameObject dialogPosition;

    [BoxGroup("AI")]
    public bool flip = true;

    [HideInInspector]
    public Character target;

    [HideInInspector]
    public Character partner;

    private bool isSleeping = true;

    private void Start()
    {
        init();
        this.target = null;
    }
    #region Animation, StateMachine


    private new void Update()
    {
        base.Update();

        if(this.target != null && this.isSleeping)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "WakeUp");
            this.isSleeping = false;
        }
        else if(this.target == null && !this.isSleeping)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Sleep");
            this.isSleeping = true;
        }
    }


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
