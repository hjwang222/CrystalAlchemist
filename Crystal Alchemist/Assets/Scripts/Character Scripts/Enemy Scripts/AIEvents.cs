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
public class AIPhase
{
    public List<AIAction> actions;
}

[System.Serializable]
public class AIEvent
{
    [BoxGroup("Trigger")]
    public List<AITrigger> trigger;

    [BoxGroup("Trigger")]
    public bool repeatEvent = false;

    [BoxGroup("Trigger")]
    public bool requireAll = false;

    [BoxGroup("Actions")]
    public List<AIAction> actions;

    [HideInInspector]
    public float startTime = 0;

    [HideInInspector]
    public bool isActive = true;
}

[System.Serializable]
public class AIAction
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
public class AITrigger
{
    [SerializeField]
    public AIEventType type;

    [ShowIf("type", AIEventType.time)]
    public float time;

    [ShowIf("type", AIEventType.life)]
    public float life;

    [ShowIf("type", AIEventType.range)]
    public List<Collider2D> collider;
}

#endregion


public class AIEvents : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private Enemy enemy;

    [SerializeField]
    private List<AIPhase> phases = new List<AIPhase>();

    [SerializeField]
    private List<AIEvent> events = new List<AIEvent>();


    private List<AIAction> activeRotation = new List<AIAction>();
    private List<AIAction> eventRotation = new List<AIAction>();

    private float timeElapsed;

    private bool startAI = false;
    private int actionsIndex;
    private int eventIndex;

    private float globalCoolDown = 0;



    private void Start()
    {
        if (this.enemy.initializeSkill != null) useSkillInstantly(this.enemy.initializeSkill);

        if (phases.Count > 0) this.activeRotation = this.phases[0].actions;
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
            this.startAI = true;
        }

        if (this.startAI)
        {
            this.timeElapsed += (Time.deltaTime * this.enemy.timeDistortion);

            //Event
            checkEvents();

            if(this.globalCoolDown <= 0)
            {
                if(this.eventRotation.Count > 0)
                {
                    this.eventIndex = doRotation(this.eventRotation, this.eventIndex, true);  
                }
                else if (this.activeRotation.Count > 0)
                {
                    this.actionsIndex = doRotation(this.activeRotation, this.actionsIndex, false);
                }
            }
            else
            {
                this.globalCoolDown -= (Time.deltaTime * this.enemy.timeDistortion);
            }
        }
    }




    #region Events

    private void checkEvents()
    {
        for(int i = 0; i < this.events.Count; i++) 
        {
            AIEvent temp = this.events[i];

            if (isTriggered(this.events[i]))
            {
                this.eventRotation.AddRange(this.events[i].actions);
                this.globalCoolDown = 0;

                if (!temp.repeatEvent)
                {
                    temp.isActive = false;
                }
                else
                {
                    temp.startTime = this.timeElapsed;
                }
            }
        }
    }

    private bool isTriggered(AIEvent elem)
    {
        int success = 0;

        if (elem.isActive)
        {
            foreach (AITrigger triggerElem in elem.trigger)
            {
                if (triggerElem.type == AIEventType.time && this.timeElapsed >= (triggerElem.time + elem.startTime))
                {
                    if (!elem.requireAll) return true;
                    else success++;
                }
                else if (triggerElem.type == AIEventType.life && this.enemy.life <= (this.enemy.maxLife * triggerElem.life / 100))
                {
                    if (!elem.requireAll) return true;
                    else success++;
                }
                else if (triggerElem.type == AIEventType.range)
                {
                    if (!elem.requireAll) return true;
                    else success++;
                }
            }

            if (elem.requireAll && success == elem.trigger.Count) return true;
        }

        return false;
    }

    public void resetAllEvents()
    {
        this.activeRotation = this.phases[0].actions;
        this.startAI = false;

        for (int i = 0; i < this.events.Count; i++)
        {
            AIEvent elem = this.events[i];
            if (!elem.isActive)
            {
                elem.isActive = true;
                elem.startTime = 0;
            }
        }
    }

    #endregion




    private int doRotation(List<AIAction> actionList, int index, bool clearList)
    {
        AIAction action = actionList[index];

        useAction(action);

        if (index < actionList.Count - 1)
        {
            return index += 1; //next Action
        }
        else
        {
            if (clearList)
            {
                actionList.Clear(); //All Events done, clear list
            }

            return 0; //repeat
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
                int currentAmountOfSameSkills = Utilities.Skill.getAmountOfSameSkills(skill, this.enemy.activeSkills);

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
            StandardSkill usedSkill = Utilities.Skill.instantiateSkill(action.skill, this.enemy, this.enemy.target);
        }
        else if (action.type == AIActionType.move)
        {
            //move enemy to position            
        }
        else if (action.type == AIActionType.dialog)
        {
            //show text bubble
            Debug.Log(action.dialogText);           
        }
        else if (action.type == AIActionType.sequence)
        {
            Instantiate(action.sequence);            
        }
        else if (action.type == AIActionType.transition)
        {
            //switch phase
            this.actionsIndex = 0;
            this.activeRotation = this.phases[action.phase].actions;
        }
        else if (action.type == AIActionType.wait)
        {
            //wait            
        }

        this.globalCoolDown = action.globalCooldown;
    }
    



}

