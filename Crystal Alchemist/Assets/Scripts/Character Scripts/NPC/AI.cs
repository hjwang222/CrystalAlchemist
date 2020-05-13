using UnityEngine;
using Sirenix.OdinInspector;

public class AI : NonPlayer
{
    [BoxGroup("Easy Access")]
    public AIAggroSystem aggroGameObject;

    [BoxGroup("AI")]
    public bool flip = true;

    [HideInInspector]
    public Character target;

    [HideInInspector]
    public Character partner;

    [HideInInspector]
    public RangeTriggered rangeTriggered;


    private bool isSleeping = true;

    public override void Awake()
    {
        base.Awake();
        this.target = null;
    }
    #region Animation, StateMachine


    public override void Update()
    {
        base.Update();

        if(this.target != null && this.isSleeping)
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
            this.isSleeping = false;
        }
        else if(this.target == null && !this.isSleeping)
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, "Sleep");
            this.isSleeping = true;
        }
    }

    public void changeState(CharacterState newState)
    {
        if (this.values.currentState != newState) this.values.currentState = newState;        
    }

    #endregion

}
