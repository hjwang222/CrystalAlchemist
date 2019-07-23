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
    [Title("$phaseName", "", bold: true)]
    [FoldoutGroup("$phaseName", Expanded = false)]
    public string phaseName;

    [SerializeField]
    [FoldoutGroup("$phaseName", Expanded = false)]
    private string phaseDescription;

    [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
    [FoldoutGroup("$phaseName", Expanded = false)]
    public List<AIAction> actions;
}

[System.Serializable]
public class AIEvent
{
    [Title("$eventName", "", bold: true)]
    [FoldoutGroup("$eventName", Expanded = false)]
    [SerializeField]
    private string eventName;

    [SerializeField]
    [FoldoutGroup("$eventName", Expanded = false)]
    private string eventDescription;

    [FoldoutGroup("$eventName", Expanded = false)]
    [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
    public List<AITrigger> trigger;

    [FoldoutGroup("$eventName", Expanded = false)]
    [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
    public List<AIAction> actions;

    [FoldoutGroup("$eventName", Expanded = false)]
    [HorizontalGroup("$eventName/Tab0", 0.5f, LabelWidth = 100)]
    public bool repeatEvent = false;

    [FoldoutGroup("$eventName", Expanded = false)]
    [HorizontalGroup("$eventName/Tab0", 0.5f, LabelWidth = 75)]
    public bool requireAll = false;

    [HideInInspector]
    public float startTime = 0;

    [HideInInspector]
    public bool isActive = true;
}

[System.Serializable]
public class AIAction
{
    [TableColumnWidth(75)]
    [VerticalGroup("Type")]
    [HideLabel]
    public AIActionType type;

    [ShowIf("type", AIActionType.move)]
    [TableColumnWidth(150)]
    [VerticalGroup("Properties")]
    [LabelWidth(75)]
    public Vector2 position;

    [ShowIf("type", AIActionType.dialog)]
    [VerticalGroup("Properties")]
    [TextArea]
    [HideLabel]
    public string dialogText;

    [ShowIf("type", AIActionType.wait)]
    [VerticalGroup("Properties")]
    [LabelWidth(75)]
    public float delay;

    [ShowIf("type", AIActionType.transition)]
    [VerticalGroup("Properties")]
    [LabelWidth(85)]
    public string nextPhase;

    [ShowIf("type", AIActionType.skill)]
    [VerticalGroup("Type")]
    [HideLabel]
    public StandardSkill skill;

    [ShowIf("type", AIActionType.skill)]
    [HorizontalGroup("Properties/Tab1", 0.5f, LabelWidth = 45)]
    public float cast = -1;

    [ShowIf("type", AIActionType.skill)]
    [HorizontalGroup("Properties/Tab1", 0.5f, LabelWidth = 30)]
    public float cD = -1;

    [ShowIf("type", AIActionType.skill)]
    [HorizontalGroup("Properties/Tab2", 0.5f, LabelWidth = 45)]
    public int amount = 0;

    [ShowIf("type", AIActionType.skill)]
    [HorizontalGroup("Properties/Tab2", 0.5f, LabelWidth = 50)]
    public bool castGroup = false;

    [ShowIf("type", AIActionType.sequence)]
    [VerticalGroup("Type")]
    [HideLabel]
    public GameObject sequence;

    [TableColumnWidth(15)]
    [VerticalGroup("GCD")]
    [HideLabel]
    public float gcd = 2.5f;


}

[System.Serializable]
public class AITrigger
{
    [TableColumnWidth(75)]
    [VerticalGroup("Type")]
    [HideLabel]
    [SerializeField]
    public AIEventType type;

    [ShowIf("type", AIEventType.time)]
    [VerticalGroup("Value")]
    [HideLabel]
    [SerializeField]
    public float time;

    [ShowIf("type", AIEventType.life)]
    [VerticalGroup("Value")]
    [HideLabel]
    [SerializeField]
    public float life;

    [ShowIf("type", AIEventType.range)]
    [VerticalGroup("Value")]
    [HideLabel]
    [SerializeField]
    public List<Collider2D> collider;
}

#endregion


public class AIEvents : MonoBehaviour
{
    #region Attributes

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private Enemy enemy;

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private CastBar castbar;

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private MiniDialogBox box;

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
    private AIAction activeAction;
    private CastBar activeCastBar;
    private MiniDialogBox activeDialog;
    private int counter;

    #endregion


    #region Unity

    private void Start()
    {
        resetAllEvents();

        Utilities.Skill.instantiateSkill(this.enemy.initializeSkill, this.enemy);

        if (phases.Count > 0) this.activeRotation = this.phases[0].actions;
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

            if (this.activeAction != null)
            {
                if (this.activeAction.skill != null)
                {
                    if (this.activeAction.cast >= 0) this.activeAction.skill.cast = this.activeAction.cast;
                    if (this.activeAction.cD >= 0) this.activeAction.skill.cooldown = this.activeAction.cD;
                }

                casting();
            }
            else
            {
                setNextAction();
            }
        }
    }

    private void casting()
    {
        if (this.activeAction.type == AIActionType.skill
                    && this.activeAction.skill != null)
        {
            if (this.activeAction.skill.holdTimer < this.activeAction.skill.cast)
            {
                this.activeAction.skill.holdTimer += (Time.deltaTime * this.enemy.timeDistortion * this.enemy.spellspeed);

                if (this.activeCastBar == null)
                {
                    GameObject temp = Instantiate(this.castbar.gameObject, this.transform.position, Quaternion.identity, this.transform);
                    //temp.hideFlags = HideFlags.HideInHierarchy;
                    this.activeCastBar = temp.GetComponent<CastBar>();
                    this.activeCastBar.target = this.enemy;
                    this.activeCastBar.skill = this.activeAction.skill;
                }
                else
                {
                    this.activeCastBar.showCastBar();
                }
            }
            else
            {
                //cast done, use action
                useAction(this.activeAction);
            }
        }
        else
        {
            useAction(this.activeAction);
        }
    }

    private void setNextAction()
    {
        //can do next action?
        if (this.globalCoolDown <= 0)
        {
            this.globalCoolDown = 0;

            if (this.eventRotation.Count > 0)
            {
                //Do events
                this.eventIndex = doRotation(this.eventRotation, this.eventIndex, true);
            }
            else if (this.activeRotation.Count > 0)
            {
                //Do current rotation
                this.actionsIndex = doRotation(this.activeRotation, this.actionsIndex, false);
            }
        }
        else
        {
            this.globalCoolDown -= (Time.deltaTime * this.enemy.timeDistortion);
        }
    }

    #endregion


    #region Events

    private void checkEvents()
    {
        for (int i = 0; i < this.events.Count; i++)
        {
            AIEvent temp = this.events[i];

            if (isTriggered(this.events[i]))
            {
                this.eventRotation.AddRange(this.events[i].actions);

                this.globalCoolDown = 0;
                this.counter = 0;
                this.activeAction = null;
                if (this.actionsIndex > 0) this.actionsIndex -= 1;

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

        clearAction();

        for (int i = 0; i < this.events.Count; i++)
        {
            AIEvent elem = this.events[i];
            if (!elem.isActive)
            {
                elem.isActive = true;
                elem.startTime = 0;
            }
        }

        this.eventIndex = 0;
    }

    #endregion


    #region use Action

    private int doRotation(List<AIAction> actionList, int index, bool clearList)
    {
        AIAction action = actionList[index];

        this.activeAction = action;
        this.counter = action.amount;

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

    private void clearAction()
    {
        if (this.activeCastBar != null) this.activeCastBar.destroyIt();
        if (this.activeAction != null && this.activeAction.skill != null) this.activeAction.skill.holdTimer = 0;
        this.activeAction = null;
        this.actionsIndex = 0;
        this.counter = 0;
        this.globalCoolDown = 0;
    }

    private void useAction(AIAction action)
    {
        bool actionUsed = false;

        if (action.type == AIActionType.skill && skillCanBeUsed(action.skill))
        {
            //useskill
            StandardSkill usedSkill = Utilities.Skill.instantiateSkill(action.skill, this.enemy, this.enemy.target);
            action.skill.cooldownTimeLeft = action.skill.cooldown;
            actionUsed = true;
        }
        else if (action.type == AIActionType.move)
        {
            //move enemy to position    
            actionUsed = true;
        }
        else if (action.type == AIActionType.dialog && this.activeDialog == null)
        {
            //TODO: Besser set Active
            GameObject dialog = Instantiate(this.box.gameObject, this.enemy.transform.position, Quaternion.identity, this.enemy.transform);
            dialog.GetComponent<MiniDialogBox>().setText(action.dialogText);
            this.activeDialog = dialog.GetComponent<MiniDialogBox>();

            actionUsed = true;
        }
        else if (action.type == AIActionType.sequence)
        {
            Instantiate(action.sequence);
            actionUsed = true;
        }
        else if (action.type == AIActionType.transition)
        {
            //switch phase

            clearAction();
            this.activeRotation = getPhaseByName(action.nextPhase);
            actionUsed = true;
        }
        else if (action.type == AIActionType.wait)
        {
            //wait
            actionUsed = true;
        }

        if (actionUsed)
        {
            this.counter--;
        }

        if ((this.activeAction != null && !this.activeAction.castGroup) || this.counter <= 0)
        {
            if (this.activeAction != null && this.activeAction.skill != null) this.activeAction.skill.holdTimer = 0;
            this.counter = 0;
            if (this.activeCastBar != null) this.activeCastBar.destroyIt();

            this.globalCoolDown = action.gcd;
            this.activeAction = null;
        }
    }

    private List<AIAction> getPhaseByName(string value)
    {
        foreach (AIPhase phase in this.phases)
        {
            if (phase.phaseName.ToUpper() == value.ToUpper()) return phase.actions;
        }

        return null;
    }

    #endregion

}

