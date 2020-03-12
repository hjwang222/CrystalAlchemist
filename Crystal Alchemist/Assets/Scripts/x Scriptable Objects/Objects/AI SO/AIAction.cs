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
    animation, 
    repeat
}

[CreateAssetMenu(menuName = "AI/AI Action")]
public class AIAction : ScriptableObject
{
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

    public void useAction(AI npc)
    {
        switch (this.type)
        {
            case AIActionType.ability: UseSkill(npc); break;
            case AIActionType.sequence: UseSequence(npc); break;
            case AIActionType.kill: npc.KillIt(); break;
            case AIActionType.animation: CustomUtilities.UnityUtils.SetAnimatorParameter(npc.animator, this.animation); break;
            case AIActionType.cannotDie: npc.setCannotDie(this.canDie); break;
            case AIActionType.invincible: npc.setInvincible(this.duration, false); break;
            case AIActionType.movement: break;
            case AIActionType.switchPhase: break;
            case AIActionType.wait: break;
            case AIActionType.repeat: break;
        }  
    }

    private void UseSkill(AI npc)
    {
        Skill usedSkill = CustomUtilities.Skills.instantiateSkill(this.ability.skill, npc, npc.target);
        if (this.overrideCastTime) this.ability.castTime = this.castTime;
        if (this.overrideCooldown) this.ability.cooldown = this.cooldown;
        this.ability.ResetCoolDown();
    }

    private void UseSequence(AI npc)
    {
        SkillSequence sequence = Instantiate(this.sequence);
        sequence.setSender(npc);
        sequence.setTarget(npc.target);
        sequence.setPosition(this.sequencePositionType, this.sequencePosition);
    }
}