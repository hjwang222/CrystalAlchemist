using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class AI : NonPlayer
{
    [BoxGroup("AI")]
    public bool flip = true;

    [HideInInspector]
    public Character target;

    [HideInInspector]
    public Dictionary<Character, float[]> aggroList = new Dictionary<Character, float[]>();

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

    public void InitializeAddSpawn(Character target)
    {
        this.IsSummoned = true;
        this.stats = Instantiate(this.stats);
        this.stats.hasRespawn = false;
        this.target = target;
        this.transform.SetParent(null);
    }

    public override void Start()
    {
        base.Start();

        this.GetComponent<AICombat>().Initialize();
        AIComponent[] components = this.GetComponents<AIComponent>();
        for (int i = 0; i < components.Length; i++) components[i].Initialize();
    }

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

        this.GetComponent<AICombat>().Updating();
        AIComponent[] components = this.GetComponents<AIComponent>();
        for (int i = 0; i < components.Length; i++) components[i].Updating();
    }

    public void changeState(CharacterState newState)
    {
        if (this.values.currentState != newState) this.values.currentState = newState;        
    }

    #endregion

}
