using UnityEngine;
using Sirenix.OdinInspector;

public enum AIActionType
{
    movement,
    dialog,
    wait,
    ability,
    sequence,
    switchPhase,
    invincible,
    kill,
    cannotDie,
    animation
}

[CreateAssetMenu(menuName = "AI/AI Action")]
public class AIAction : ScriptableObject
{
    #region Attributes

    [BoxGroup("Type")]
    [SerializeField]
    private AIActionType type;

    [ShowIf("type", AIActionType.movement)]
    [BoxGroup("Properties")]
    [SerializeField]
    private Vector2 position;

    [ShowIf("type", AIActionType.dialog)]
    [BoxGroup("Properties")]
    [SerializeField]
    private string de;

    [ShowIf("type", AIActionType.dialog)]
    [BoxGroup("Properties")]
    [SerializeField]
    private string en;

    [HideIf("type", AIActionType.movement)]
    [HideIf("type", AIActionType.switchPhase)]
    [HideIf("type", AIActionType.animation)]
    [HideIf("type", AIActionType.ability)]
    [HideIf("type", AIActionType.sequence)]
    [HideIf("type", AIActionType.kill)]
    [BoxGroup("Properties")]
    [SerializeField]
    private float duration = 4f;

    [ShowIf("type", AIActionType.cannotDie)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool canDie = false;

    [ShowIf("type", AIActionType.switchPhase)]
    [BoxGroup("Properties")]
    [SerializeField]
    private AIPhase nextPhase;

    [ShowIf("type", AIActionType.animation)]
    [BoxGroup("Properties")]
    [SerializeField]
    private string animation;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private Ability ability;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideCastTime;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideCastTime")]
    [BoxGroup("Properties")]
    [SerializeField]
    private float castTime = -1;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private bool overrideCooldown;

    [ShowIf("type", AIActionType.ability)]
    [ShowIf("overrideCooldown")]
    [BoxGroup("Properties")]
    [SerializeField]
    private float cooldown = -1;

    [ShowIf("type", AIActionType.ability)]
    [BoxGroup("Properties")]
    [SerializeField]
    private int repeatAmount = 0;

    [ShowIf("type", AIActionType.sequence)]
    [BoxGroup("Properties")]
    [SerializeField]
    private SkillSequence sequence;

    [ShowIf("type", AIActionType.sequence)]
    [BoxGroup("Properties")]
    [SerializeField]
    private modificationType sequencePositionType;

    [ShowIf("type", AIActionType.sequence)]
    [ShowIf("sequencePositionType", modificationType.fix)]
    [BoxGroup("Properties")]
    [SerializeField]
    private Vector2 sequencePosition;

    #endregion

    private float waitTimer = 0;

    //casting
    //interrupted


    #region Main Functions

    public void Initialize(AI npc)
    {
        switch (this.type)
        {
            case AIActionType.ability: StartSkill(); break;
            case AIActionType.kill: StartKill(npc); break;
            case AIActionType.animation: StartAnimation(npc); break;
            case AIActionType.cannotDie: StartCannotDie(npc); break;
            case AIActionType.invincible: StartInvinicible(npc); break;
            case AIActionType.wait: StartWait(); break;
            case AIActionType.dialog: StartDialog(npc); break;
            case AIActionType.switchPhase: StartPhase(npc); break;
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

    #endregion


    #region Ability

    private void StartSkill()
    {
        Ability ability = Instantiate(this.ability);
        this.ability = ability;
    }

    private void UpdateSkill(AI npc)
    {
        //casting here
        Skill usedSkill = CustomUtilities.Skills.instantiateSkill(this.ability.skill, npc, npc.target);
        if (this.overrideCastTime) this.ability.castTime = this.castTime;
        if (this.overrideCooldown) this.ability.cooldown = this.cooldown;
        this.ability.ResetCoolDown();
        Destroy(this);
    }

    #endregion


    #region Sequence

    private void UpdateSequence(AI npc)
    {
        //casting here
        SkillSequence sequence = Instantiate(this.sequence);
        sequence.setSender(npc);
        sequence.setTarget(npc.target);
        sequence.setPosition(this.sequencePositionType, this.sequencePosition);
        Destroy(this);
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
        else Destroy(this);
    }

    #endregion


    #region Kill

    private void StartKill(AI npc)
    {
        npc.KillIt();
        Destroy(this);
    }

    #endregion


    #region Animation

    private void StartAnimation(AI npc)
    {
        CustomUtilities.UnityUtils.SetAnimatorParameter(npc.animator, this.animation);
        Destroy(this);
    }

    #endregion


    #region Invincible

    private void StartInvinicible(AI npc)
    {
        npc.setInvincible(this.duration, false);
        Destroy(this);
    }

    #endregion


    #region CannotDie

    private void StartCannotDie(AI npc)
    {
        npc.setCannotDie(this.canDie);
        Destroy(this);
    }

    #endregion


    #region Dialog

    private void StartDialog(AI npc)
    {
        string text = CustomUtilities.Format.getLanguageDialogText(this.de, this.en);
        npc.GetComponent<AIEvents>().ShowDialog(text, this.duration);
        this.waitTimer = this.duration;
    }

    private void UpdateDialog()
    {
        if (this.waitTimer > 0) this.waitTimer -= Time.deltaTime;
        else Destroy(this);
    }

    #endregion


    #region PhaseTransition

    private void StartPhase(AI npc)
    {
        npc.GetComponent<AIEvents>().SwitchPhase(this.nextPhase);
    }

    #endregion


    private void OnDestroy()
    {
        
    }
}