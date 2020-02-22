using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AI : Character
{
    [Required]
    [BoxGroup("Easy Access")]
    public AIAggroSystem aggroGameObject;

    [BoxGroup("AI")]
    public bool flip = true;

    [HideInInspector]
    public Character target;

    [HideInInspector]
    public Character partner;

    private bool isSleeping = true;

    private void Awake()
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
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "WakeUp");
            this.isSleeping = false;
        }
        else if(this.target == null && !this.isSleeping)
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "Sleep");
            this.isSleeping = true;
        }
    }

    public void changeState(CharacterState newState)
    {
        if (this.currentState != newState) this.currentState = newState;        
    }

    #endregion

}
