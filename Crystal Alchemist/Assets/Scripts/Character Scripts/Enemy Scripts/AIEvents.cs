using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


#region struct und enums

public enum AIActionType
{
    move,
    dialog,
    wait,
    skill,
    sequence,
    transition
}

public enum AIEventType
{
    time,
    life,
    range
}

[System.Serializable]
public struct AIRotation
{
    public int phase;

    public List<AIAction> actions;
}

[System.Serializable]
public struct AIAction
{
    public AIActionType type;

    [ShowIf("type", AIActionType.move)]
    public Vector2 position;

    [ShowIf("type", AIActionType.dialog)]
    [TextArea]
    public string dialogText;

    [ShowIf("type", AIActionType.wait)]
    public float delay;

    [ShowIf("type", AIActionType.transition)]
    public int phase;

    [ShowIf("type", AIActionType.skill)]
    public StandardSkill skill;

    [ShowIf("type", AIActionType.sequence)]
    public GameObject sequence;

    public float globalCooldown;
}

[System.Serializable]
public struct AITrigger
{
    [SerializeField]
    public AIEventType type;

    [ShowIf("type", AIEventType.time)]
    public float time;

    [ShowIf("type", AIEventType.life)]
    public float life;

    [ShowIf("type", AIEventType.range)]
    public List<Collider2D> collider;

    public List<AIAction> actions;

    public bool isActive;
}

#endregion


public class AIEvents : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private Enemy enemy;

    [SerializeField]
    private List<AIRotation> rotation = new List<AIRotation>();

    [SerializeField]
    private List<AITrigger> events;

    private List<AIAction> activeRotation = new List<AIAction>();
    private List<AIAction> additionalActions = new List<AIAction>();

    private float timeElapsed;
    private bool canUseAction = true;

    private int actionsIndex;
    //private bool startAI = false;

    private void Start()
    {
        if (this.enemy.initializeSkill != null) useSkillInstantly(this.enemy.initializeSkill);

        if (rotation.Count > 0) this.activeRotation = this.rotation[0].actions;
    }

    private void useSkillInstantly(StandardSkill skill)
    {
        if (this.enemy.activeCastbar != null && skill.holdTimer == 0) this.enemy.activeCastbar.destroyIt();

        if (skill.cooldownTimeLeft > 0)
        {
            skill.cooldownTimeLeft -= (Time.deltaTime * this.enemy.timeDistortion);
        }
        else
        {
            int currentAmountOfSameSkills = Utilities.Skill.getAmountOfSameSkills(skill, this.enemy.activeSkills);

            if (currentAmountOfSameSkills < skill.maxAmounts
                && this.enemy.getResource(skill.resourceType, skill.item) + skill.addResourceSender >= 0)
            {
                if (!skill.isRapidFire && !skill.keepHoldTimer) skill.holdTimer = 0;

                skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown

                StandardSkill temp = Utilities.Skill.instantiateSkill(skill, this.enemy, null, 1);

            }
        }
    }

    private void Update()
    {
        if (enemy.target != null)
        {
            this.timeElapsed += (Time.deltaTime * this.enemy.timeDistortion);
            //Ziel erfasst -> Angriff!

            //Event
            //checkEvents();

            //Event-Actions
            //useEventActions();

            //Rotation
            //useActions(this.activeRotation);
        }
    }


    /*
     * Trigger:
     * Werden ausgelöst wenn bestimme Voraussetzungen erfüllt sind
     *  
     * 
     * Events:
     * Zusätzliche Aktionen, die durch Trigger ausgelöst werden
     * 
     * 
     * Rotation:
     * Festgelegte Reihenfolge an Sequenzen oder Skills, die ständig wiederholten werden
     * /



    /*
    private void checkEvents()
    {
        foreach (AITrigger elem in this.events)
        {
            if (isTriggered(elem)) addAdditionalActions(elem.actions);
        }
    }

    private void useEventActions()
    {
        foreach (AIAction action in this.additionalActions)
        {
            useAction(action);
        }

        this.additionalActions.Clear();
    }

    private void addAdditionalActions(List<AIAction> actions)
    {
        //check cooldown and maxAmounts
        this.additionalActions.AddRange(actions);
    }

    private void useActions(List<AIAction> actions)
    {
        if (this.canUseAction && actions.Count > 0)
        {
            AIAction action = actions[this.actionsIndex];
            useAction(action);
            StartCoroutine(waitCo(action.globalCooldown));
            this.actionsIndex++;
        }
    }

    private bool skillCanBeUsed(StandardSkill skill)
    {
        if (skill != null)
        {
            if (skill.cooldownTimeLeft > 0)
            {
                skill.cooldownTimeLeft -= (Time.deltaTime * this.enemy.timeDistortion * this.enemy.spellspeed);
            }
            else if (this.enemy.currentState != CharacterState.interact
                  && this.enemy.currentState != CharacterState.inDialog
                  && this.enemy.currentState != CharacterState.inMenu)
            {
                int currentAmountOfSameSkills = this.enemy.getAmountOfSameSkills(skill);

                if (currentAmountOfSameSkills < skill.maxAmounts)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void useAction(AIAction action)
    {
        if (action.type == AIActionType.skill 
            && skillCanBeUsed(action.skill))
        {
            //useskill
            StandardSkill usedSkill = Utilities.Skill.instantiateSkill(action.skill, this.enemy);               
        }
        else if (action.type == AIActionType.wait)
        {
            //wait
            StartCoroutine(waitCo(action.delay));
        }

        //moveEnemy
        //showDialog

    }

    private void switchPhase()
    {
        //this.rotation = new phase
        //index = 0;
    }

    private IEnumerator waitCo(float delay)
    {
        this.canUseAction = false;
        yield return new WaitForSeconds(delay);
        this.canUseAction = true;
    }

    private bool isTriggered(AITrigger elem)
    {
        if (elem.isActive)
        {
            if (elem.type == AIEventType.time && this.timeElapsed >= elem.time)
            {
                return true;
            }
            else if (elem.type == AIEventType.life && this.enemy.life <= (this.enemy.life * elem.life/100))
            {
                return true;
            }
            else if (elem.type == AIEventType.range)
            {
                return true;
            }
        }

        return false;
    }*/
}

