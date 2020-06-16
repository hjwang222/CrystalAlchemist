using UnityEngine;
using Sirenix.OdinInspector;

public enum AIActionType
{
    movement,
    dialog,
    wait,
    ability,
    sequence,
    startPhase,
    endPhase,
    invincible,
    kill,
    cannotDie,
    animation,
    signal
}

[System.Serializable]
public class AIAction
{
    #region Attributes

    [BoxGroup("Properties")]
    [SerializeField]
    private AIActionType type;

    [ShowIf("type", AIActionType.movement)]
    [BoxGroup("Properties")]
    [SerializeField]
    private Vector2 position;

    //Movement: Fixed, follow, path, evade

    [ShowIf("type", AIActionType.dialog)]
    [BoxGroup("Properties")]
    [SerializeField]
    private string translationID;

    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.startPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.cannotDie)]
    [HideIf("type", AIActionType.ability)]
    [HideIf("type", AIActionType.sequence)]
    [HideIf("type", AIActionType.kill)]
    [HideIf("type", AIActionType.signal)]
    [HideIf("type", AIActionType.invincible)]
    [BoxGroup("Properties")]
    [SerializeField]
    private float duration = 4f;

    [ShowIf("type", AIActionType.startPhase)]
    [BoxGroup("Properties")]
    [SerializeField]
    private AIPhase nextPhase;

    [ShowIf("type", AIActionType.animation)]
    [BoxGroup("Properties")]
    [SerializeField]
    private string animations;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    [OnValueChanged("AbilityChanged")]
    private Ability ability;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideCastTime = false;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideCastTime")]
    [Indent(1)]
    [BoxGroup("Properties")]
    [SerializeField]
    private float castTime = 0;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideShowCastBar = false;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideShowCastBar")]
    [Indent(1)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool showCastBar = false;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideCooldown;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideCooldown")]
    [Indent(1)]
    [BoxGroup("Properties")]
    [SerializeField]
    private float cooldown = 0;

    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.startPhase)]
    [HideIf("type", AIActionType.endPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.cannotDie)]
    [HideIf("type", AIActionType.kill)]
    [HideIf("type", AIActionType.dialog)]
    [HideIf("type", AIActionType.wait)]
    [HideIf("type", AIActionType.signal)]
    [HideIf("type", AIActionType.invincible)]
    [BoxGroup("Properties")]
    [SerializeField]
    [OnValueChanged("RepeatChanged")]
    private bool repeat = false;

    [HideIf("type", AIActionType.sequence)]
    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.startPhase)]
    [HideIf("type", AIActionType.endPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.cannotDie)]
    [HideIf("type", AIActionType.kill)]
    [HideIf("type", AIActionType.dialog)]
    [HideIf("type", AIActionType.wait)]
    [HideIf("type", AIActionType.signal)]
    [HideIf("type", AIActionType.invincible)]
    [BoxGroup("Properties")]
    [ShowIf("repeat")]
    [SerializeField]
    private bool keepCast = false;

    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.startPhase)]
    [HideIf("type", AIActionType.endPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.cannotDie)]
    [HideIf("type", AIActionType.kill)]
    [HideIf("type", AIActionType.dialog)]
    [HideIf("type", AIActionType.wait)]
    [HideIf("type", AIActionType.signal)]
    [HideIf("type", AIActionType.invincible)]
    [ShowIf("repeat")]
    [BoxGroup("Properties")]
    [SerializeField]
    [MinValue(1)]
    private int amount = 1;

    [ShowIf("repeat")]
    [BoxGroup("Properties")]
    [SerializeField]
    private float delay = 0f;

    [ShowIf("type", AIActionType.sequence)]
    [BoxGroup("Properties")]
    [SerializeField]
    private SkillSequence sequence;

    [HideIf("type", AIActionType.ability)]
    [HideIf("type", AIActionType.sequence)]
    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.startPhase)]
    [HideIf("type", AIActionType.endPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.kill)]
    [HideIf("type", AIActionType.dialog)]
    [HideIf("type", AIActionType.wait)]
    [HideIf("type", AIActionType.signal)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool value;

    [ShowIf("type", AIActionType.signal)]
    [BoxGroup("Properties")]
    [SerializeField]
    private SimpleSignal signal;

    [HideIf("type", AIActionType.wait)]
    [HideIf("type", AIActionType.kill)]
    [BoxGroup("Properties")]
    [SerializeField]
    private float wait = 0f;

    #endregion

    private float elapsed = 0;
    private bool isActive = true;
    private Ability tempAbility;
    private int counter = 0;

    //TODO: Add status effect

    #region Main Functions

    public AIAction(float duration, AI npc)
    {
        this.type = AIActionType.wait;
        this.duration = duration;
        Initialize(npc);
    }

    public void Initialize(AI npc)
    {
        this.isActive = true;

        switch (this.type)
        {
            case AIActionType.ability: StartSkill(npc); break;
            case AIActionType.sequence: StartSequence(); break;
            case AIActionType.kill: StartKill(npc); break;
            case AIActionType.animation: StartAnimation(npc); break;
            case AIActionType.cannotDie: StartCannotDie(npc); break;
            case AIActionType.invincible: StartInvinicible(npc); break;
            case AIActionType.wait: StartWait(); break;
            case AIActionType.dialog: StartDialog(npc); break;
            case AIActionType.startPhase: StartPhase(npc); break;
            case AIActionType.endPhase: EndPhase(npc); break;
            case AIActionType.signal: StartSignal(); break;
        }
    }

    public void Updating(AI npc)
    {
        switch (this.type)
        {
            case AIActionType.ability: UpdateSkill(npc); break;
            case AIActionType.sequence: UpdateSequence(npc); break;
            case AIActionType.wait: UpdateWait(); break;
            case AIActionType.dialog: UpdateDialog(); break;
        }
    }

    public void Disable(AI npc)
    {
        switch (this.type)
        {
            case AIActionType.ability: DisableSkill(npc); break;
            case AIActionType.dialog: DisableDialog(); break;
        }
    }

    public bool isDialog()
    {
        return (this.type == AIActionType.dialog);
    }

    public bool getActive()
    {
        return this.isActive;
    }

    public float GetWait()
    {
        if (this.type == AIActionType.wait) return 0;
        return this.wait;
    }

    #endregion


    #region Ability

    private void StartSkill(AI npc)
    {
        this.tempAbility = AbilityUtil.InstantiateAbility(this.ability);
        this.tempAbility.SetSender(npc);

        if (this.overrideCastTime) this.tempAbility.castTime = this.castTime;
        if (this.overrideShowCastBar) this.tempAbility.showCastbar = this.showCastBar;
        if (this.overrideCooldown) this.tempAbility.cooldown = this.cooldown;

        if (!this.repeat) this.amount = 1;
        this.counter = 0;
    }

    private void UpdateSkill(AI npc)
    {
        this.tempAbility.Updating();

        if (npc.values.isCharacterStunned()) this.tempAbility.ResetCharge();

        if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
        else
        {
            if (this.tempAbility.state == AbilityState.notCharged) Charge(npc);
            else if (this.tempAbility.state == AbilityState.targetRequired) CheckTargets(npc);
            else if (this.tempAbility.state == AbilityState.charged
                  || this.tempAbility.state == AbilityState.ready) UseSkill(npc);
        }

        if (this.counter >= this.amount) DisableSkill(npc);
    }

    private void Charge(AI npc)
    {
        npc.GetComponent<AIEvents>().ChargeAbility(this.tempAbility, npc, npc.target);

        if (this.tempAbility.IsTargetRequired())
            npc.GetComponent<AIEvents>().ShowTargetingSystem(this.tempAbility);        //Show Targeting System when needed
    }

    private void CheckTargets(AI npc)
    {
        if (!this.tempAbility.IsTargetRequired() && npc.target != null)
            this.tempAbility.state = AbilityState.ready; //SingleTarget
        else if (this.tempAbility.IsTargetRequired())
            this.tempAbility.state = AbilityState.ready; //Target from TargetingSystem
    }

    private void UseSkill(AI npc)
    {
        npc.GetComponent<AIEvents>().HideCastBar();

        if (this.tempAbility.IsTargetRequired()) npc.GetComponent<AIEvents>().UseAbilityOnTargets(this.tempAbility);
        else
        {
            npc.GetComponent<AIEvents>().UseAbilityOnTarget(this.tempAbility, npc.target);
            npc.GetComponent<AIEvents>().HideTargetingSystem(this.tempAbility);
        }

        this.elapsed = this.delay;
        this.counter++;

        if (!this.keepCast)
        {
            npc.GetComponent<AIEvents>().HideTargetingSystem(this.tempAbility);
            npc.GetComponent<AIEvents>().UnChargeAbility(this.tempAbility, npc); //reset Charge
        }
    }

    private void DisableSkill(AI npc)
    {
        npc.GetComponent<AIEvents>().HideTargetingSystem(this.tempAbility);
        npc.GetComponent<AIEvents>().UnChargeAbility(this.tempAbility, npc); //reset Charge
        Deactivate();
    }

    #endregion


    #region Sequence

    private void StartSequence()
    {
        if (!this.repeat) this.amount = 1;
        this.counter = 0;
    }

    private void UpdateSequence(AI npc)
    {
        if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
        else 
        {
            this.sequence.InstantiateSequence(npc);
            this.elapsed = this.delay;
            this.counter++;
        }

        if(counter >= this.amount) Deactivate();
    }

    #endregion


    #region Wait

    private void StartWait()
    {
        this.elapsed = this.duration;
    }

    private void UpdateWait()
    {
        if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
        else Deactivate();
    }

    #endregion


    #region Kill

    private void StartKill(AI npc)
    {
        npc.Dead();
        Deactivate();
    }

    #endregion


    #region Animation

    private void StartAnimation(AI npc)
    {
        AnimatorUtil.SetAnimatorParameter(npc.animator, this.animations);
        Deactivate();
    }

    #endregion


    #region Invincible

    private void StartInvinicible(AI npc)
    {
        npc.SetInvincible(this.value);
        Deactivate();
    }

    #endregion


    #region CannotDie

    private void StartCannotDie(AI npc)
    {
        npc.setCannotDie(this.value);
        Deactivate();
    }

    #endregion


    #region Dialog

    private void StartDialog(AI npc)
    {
        string text = FormatUtil.GetLocalisedText(this.translationID, LocalisationFileType.dialogs);
        npc.ShowDialog(text, this.duration);
        this.elapsed = this.duration;
    }

    private void UpdateDialog()
    {
        if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
        else Deactivate();
    }

    private void DisableDialog()
    {
        this.elapsed = 0;
    }

    #endregion


    #region PhaseTransition

    private void StartPhase(AI npc)
    {
        npc.GetComponent<AIEvents>().StartPhase(this.nextPhase);
    }

    private void EndPhase(AI npc)
    {
        npc.GetComponent<AIEvents>().EndPhase();
    }

    #endregion


    #region Signal

    private void StartSignal()
    {
        if (this.signal != null) this.signal.Raise();
        Deactivate();
    }

    #endregion

    private void Deactivate() =>  this.isActive = false;    



    private void AbilityChanged()
    {
        this.wait = (this.ability.castTime + this.delay) * this.amount;
    }

    private void RepeatChanged()
    {
        if (!this.repeat) this.amount = 1;
    }
}