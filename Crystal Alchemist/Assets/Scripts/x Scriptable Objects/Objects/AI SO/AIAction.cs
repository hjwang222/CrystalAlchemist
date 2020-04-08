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
    animation
}

//[CreateAssetMenu(menuName = "AI/AI Action")]
[System.Serializable]
public class AIAction //: ScriptableObject
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
    private string de;

    [ShowIf("type", AIActionType.dialog)]
    [BoxGroup("Properties")]
    [SerializeField]
    private string en;

    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.startPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.cannotDie)]
    [HideIf("type", AIActionType.ability)]
    [HideIf("type", AIActionType.sequence)]
    [HideIf("type", AIActionType.kill)]
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
    private Ability ability;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideCastTime = false;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideShowCastBar = false;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool showCastBar = false;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideCastTime")]
    [BoxGroup("Properties")]
    [SerializeField]
    private float castTime = 0;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideCooldown;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideCooldown")]
    [BoxGroup("Properties")]
    [SerializeField]
    private float cooldown = 0;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool repeatSkill = false;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("repeatSkill")]
    [BoxGroup("Properties")]
    [SerializeField]
    private int amount = 1;

    [ShowIf("type", AIActionType.sequence)]
    [BoxGroup("Properties")]
    [SerializeField]
    private BossSequence sequence;

    #endregion

    private float waitTimer = 0;
    private bool isActive = true;
    private Ability tempAbility;
    private int skillCounter = 0;

    //TODO: Add status effect

    #region Main Functions

    public void Initialize(AI npc)
    {
        this.isActive = true;

        switch (this.type)
        {
            case AIActionType.ability: StartSkill(); break;
            case AIActionType.kill: StartKill(npc); break;
            case AIActionType.animation: StartAnimation(npc); break;
            case AIActionType.cannotDie: StartCannotDie(npc); break;
            case AIActionType.invincible: StartInvinicible(npc); break;
            case AIActionType.wait: StartWait(); break;
            case AIActionType.dialog: StartDialog(npc); break;
            case AIActionType.startPhase: StartPhase(npc); break;
            case AIActionType.endPhase: EndPhase(npc); break;
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

    public bool isDialog()
    {
        return (this.type == AIActionType.dialog);
    }

    public bool getActive()
    {
        return this.isActive;
    }

    #endregion


    #region Ability

    private void StartSkill()
    {
        this.tempAbility = AbilityUtil.InstantiateAbility(this.ability);
        if (this.overrideCastTime) this.tempAbility.castTime = this.castTime;
        if (this.overrideShowCastBar) this.tempAbility.showCastbar = this.showCastBar;
        if (this.overrideCooldown) this.tempAbility.cooldown = this.cooldown;
        this.tempAbility.Initialize();

        if (!this.repeatSkill) this.amount = 1;
        this.skillCounter = 0;
    }

    private void UpdateSkill(AI npc)
    {
        this.tempAbility.Update();

        if (StatusEffectUtil.isCharacterStunned(npc)) this.tempAbility.ResetCharge();

        if (this.tempAbility.state == AbilityState.notCharged) Charge(npc);
        else if (this.tempAbility.state == AbilityState.targetRequired) LockOn(npc);
        else if (this.tempAbility.state == AbilityState.charged
              || this.tempAbility.state == AbilityState.ready
              || this.tempAbility.state == AbilityState.lockOn) UseSkill(npc);

        ResetCharge(npc);
    }

    private void Charge(AI npc)
    {
        npc.GetComponent<AIEvents>().ChargeAbility(this.tempAbility, npc);
    }

    private void LockOn(AI npc)
    {
        if(npc.target != null) this.tempAbility.state = AbilityState.lockOn;
    }

    private void UseSkill(AI npc)
    {
        if (ability.canUseAbility(npc))
        {
            npc.GetComponent<AIEvents>().HideCastBar();
            AbilityUtil.instantiateSkill(this.tempAbility.skill, npc, npc.target);
            this.tempAbility.ResetCoolDown();
            this.skillCounter++;
        }
    }

    private void ResetCharge(AI npc)
    {
        if (this.skillCounter >= this.amount)
        {
            npc.GetComponent<AIEvents>().UnChargeAbility(this.tempAbility, npc);
            Deactivate();            
        }        
    }

    #endregion


    #region Sequence

    private void UpdateSequence(AI npc)
    {
        //casting here       
        AbilityUtil.instantiateSequence(this.sequence, npc);
        Deactivate();
    }

    #endregion


    #region Wait

    private void StartWait()
    {
        this.waitTimer = this.duration;
    }

    private void UpdateWait()
    {
        if (this.waitTimer > 0) this.waitTimer -= Time.deltaTime;
        else Deactivate();
    }

    #endregion


    #region Kill

    private void StartKill(AI npc)
    {
        npc.KillIt();
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
        npc.setInvincible(this.duration, false);
        Deactivate();
    }

    #endregion


    #region CannotDie

    private void StartCannotDie(AI npc)
    {
        npc.setCannotDie(true);
        Deactivate();
    }

    #endregion


    #region Dialog

    private void StartDialog(AI npc)
    {
        string text = FormatUtil.getLanguageDialogText(this.de, this.en);
        npc.GetComponent<AIEvents>().ShowDialog(text, this.duration);
        this.waitTimer = this.duration;
    }

    private void UpdateDialog()
    {
        if (this.waitTimer > 0) this.waitTimer -= Time.deltaTime;
        else Deactivate();
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

    private void Deactivate()
    {
        //Destroy(this);
        this.isActive = false;
    }

}