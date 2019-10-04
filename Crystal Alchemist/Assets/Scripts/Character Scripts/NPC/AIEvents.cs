using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


#region struct und enums

public enum AIActionType
{
    movement,
    dialog,
    wait,
    skill,
    sequence,
    transition, 
    immortal,
    kill,
    animation
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
    public List<string> affectedPhases = new List<string>();

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

    [ShowIf("type", AIActionType.immortal)]
    [VerticalGroup("Properties")]
    [LabelWidth(100)]
    public bool isImmortal;

    [ShowIf("type", AIActionType.movement)]
    [TableColumnWidth(150)]
    [VerticalGroup("Properties")]
    [LabelWidth(75)]
    public Vector2 position;

    [ShowIf("type", AIActionType.dialog)]
    [VerticalGroup("Properties")]
    [LabelWidth(25)]
    public string de;

    [ShowIf("type", AIActionType.dialog)]
    [VerticalGroup("Properties")]
    [LabelWidth(25)]
    public string en;

    [ShowIf("type", AIActionType.dialog)]
    [VerticalGroup("Type")]
    [LabelWidth(75)]
    public float duration = 4f;

    [ShowIf("type", AIActionType.immortal)]
    [VerticalGroup("Type")]
    [LabelWidth(75)]
    public float dauer = 4f;

    [ShowIf("type", AIActionType.transition)]
    [VerticalGroup("Properties")]
    [LabelWidth(85)]
    public string nextPhase;

    [ShowIf("type", AIActionType.animation)]
    [VerticalGroup("Properties")]
    [LabelWidth(85)]
    public string animation;

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
    public SkillSequence sequence;

    [TableColumnWidth(15)]
    [VerticalGroup("GCD")]
    [HideLabel]
    public float gcd = 2.5f;

    [HideInInspector]
    public StandardSkill skillinstance;
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
    public List<RangeTriggered> rangeTrigger;
}

#endregion


public class AIEvents : MonoBehaviour
{
    #region Attributes

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI enemy;

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private CastBar castbar;

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private MiniDialogBox box;

    [SerializeField]
    [BoxGroup("Required")]
    private bool startImmediately = false;

    [SerializeField]
    private List<AIAction> initialActions = new List<AIAction>();

    [SerializeField]
    private List<AIPhase> phases = new List<AIPhase>();

    [SerializeField]
    private List<AIEvent> events = new List<AIEvent>();

    private AIAction activeAction;
    private AIPhase activePhase;

    private List<AIAction> activeEvents = new List<AIAction>();

    private AIAction activeDialogAction;
    private List<AIAction> activeDialogs = new List<AIAction>();

    private float timeElapsed;

    private bool AIstarted = false;
    private int actionsIndex = 0;
    private int eventIndex = 0;
    private int dialogIndex = 0;

    private float globalCoolDown = 0;
    private bool nextDialog = true;
    private bool AIstopped = false;

    private MiniDialogBox activeDialog;
    private int counter;

    #endregion


    #region Unity

    private void Start()
    {
        init();
    }

    public void init()
    {
        resetAllEvents();

        foreach (AIAction action in this.initialActions)
        {
            if (action.type != AIActionType.dialog)
            {                
                if (action.skillinstance == null && action.skill != null)
                {
                    action.skillinstance = Utilities.Skill.setSkill(this.enemy, action.skill);
                    action.skillinstance.showingIndicator = true;                    
                }

                useAction(action);
            }
            else showDialog(action);
        }

        if (phases.Count > 0)
        {
            this.activePhase = this.phases[0];
        }
    }

    private void Update()
    {
        checkStart();

        if((this.enemy.stats.characterType == CharacterType.Enemy && this.AIstarted)
            || this.enemy.stats.characterType == CharacterType.NPC) handleDialogs();

        if (this.AIstarted || this.startImmediately)
        {
            if (this.activeAction != null 
                && this.activeAction.skillinstance == null 
                && this.enemy != null
                && this.enemy.activeCastbar != null) this.enemy.activeCastbar.destroyIt();

            if (!this.AIstopped) doEvents();            
        }
    }

    private void checkStart()
    {
        if (this.enemy.target != null) this.AIstarted = true;
        else if (this.AIstarted && this.enemy.target == null)
        { 
            this.AIstarted = false;
            resetAllEvents();
        }
    }

    private IEnumerator stopCo(float duration)
    {
        this.AIstopped = true;
        yield return new WaitForSeconds(duration);
        this.AIstopped = false;
    }

    private IEnumerator waitDialogCo(float duration)
    {
        this.nextDialog = false;
        yield return new WaitForSeconds(duration);
        this.nextDialog = true;
    }

    private void handleDialogs()
    {
        if (this.nextDialog)
        {
            if (this.activeDialogAction != null
            && this.activeDialog == null)
            {
                showDialog(this.activeDialogAction);
            }
            else if (this.activeDialog == null
                && this.activeDialogAction == null)
            {
                setNextDialog();
            }
        }
    }

    private void doEvents()
    {
        if (!Utilities.StatusEffectUtil.isCharacterStunned(this.enemy))
        {
            this.timeElapsed += (Time.deltaTime * this.enemy.timeDistortion);

            //Event
            checkEvents();

            if (this.activeAction != null)
            {
                if (this.activeAction.skillinstance != null)
                {
                    if (this.activeAction.cast >= 0) this.activeAction.skillinstance.cast = this.activeAction.cast;
                    if (this.activeAction.cD >= 0) this.activeAction.skillinstance.cooldown = this.activeAction.cD;
                    if (this.activeAction.skillinstance.setTargetAutomatically) this.activeAction.skillinstance.target = this.enemy.target;
                }

                casting();
            }
            else
            {
                setNextAction();
            }
        }
        else
        {
            if(this.activeAction != null) this.enemy.resetCast(this.activeAction.skillinstance);
        }
    }

    private void setNextDialog()
    {
        if (this.activeDialogAction == null && this.activeDialogs.Count > 0)
            this.dialogIndex = setNextActionToDo(this.activeDialogs, this.dialogIndex, true);
    }

    private void casting()
    {
        if (this.activeAction.type == AIActionType.skill
         && this.activeAction.skillinstance != null)
        {
            if (this.enemy.target != null)
            {
                if (this.activeAction.skillinstance.target == null) this.activeAction.skillinstance.target = this.enemy.target;

                if (this.activeAction.skillinstance.holdTimer < this.activeAction.skillinstance.cast)
                {
                    this.activeAction.skillinstance.holdTimer += (Time.deltaTime * this.enemy.timeDistortion * this.enemy.spellspeed);

                    this.activeAction.skillinstance.showIndicator(); //Zeige Indikator beim Casten
                    this.activeAction.skillinstance.showCastingAnimation();
                    this.activeAction.skillinstance.doOnCast();

                    if (this.activeAction.skillinstance.showCastBarForEnemies)
                    {
                        if (this.enemy.activeCastbar == null)
                        {
                            GameObject temp = Instantiate(this.castbar.gameObject, this.transform.position, Quaternion.identity, this.transform);
                            //temp.hideFlags = HideFlags.HideInHierarchy;
                            this.enemy.activeCastbar = temp.GetComponent<CastBar>();
                            this.enemy.activeCastbar.target = this.enemy;
                            this.enemy.activeCastbar.skill = this.activeAction.skillinstance;
                        }
                        else
                        {
                            this.enemy.activeCastbar.showCastBar();
                        }
                    }
                }
                else
                {
                    //cast done, use action                
                    useAction(this.activeAction);
                }
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

            if (this.activeEvents.Count > 0)
            {
                //Do events
                this.eventIndex = setNextActionToDo(this.activeEvents, this.eventIndex, true);
            }
            else if (this.activePhase != null
                && this.activePhase.actions != null
                && this.activePhase.actions.Count > 0)
            {
                //Do current rotation
                this.actionsIndex = setNextActionToDo(this.activePhase.actions, this.actionsIndex, false);
            }
        }
        else
        {
            this.globalCoolDown -= (Time.deltaTime * this.enemy.timeDistortion);
        }
    }

    #endregion


    #region Events

    private void addRange(List<AIAction> actions)
    {
        foreach (AIAction action in actions)
        {
            if (action.type == AIActionType.dialog) this.activeDialogs.Add(action);
            else this.activeEvents.Add(action);
        }
    }

    private void showDialog(AIAction action)
    {
        float wait = action.gcd + action.duration;
        //Debug.Log("Talk: " + action.dialogText+" (wait "+ wait +"s)");

        GameObject dialog = Instantiate(this.box.gameObject, this.enemy.dialogPosition.transform);

        MiniDialogBox temp = dialog.GetComponent<MiniDialogBox>();
        temp.setText(Utilities.Format.getLanguageDialogText(action.de, action.en));
        temp.setDuration(action.duration);
        this.activeDialog = temp;
        this.activeDialogAction = null;
        StartCoroutine(waitDialogCo(wait));
    }

    private void checkEvents()
    {
        for (int i = 0; i < this.events.Count; i++)
        {
            AIEvent temp = this.events[i];

            if (isTriggered(this.events[i]))
            {
                //Debug.Log("TRIGGERED!");

                clearAction();
                if (this.actionsIndex > 0) this.actionsIndex -= 1;

                addRange(this.events[i].actions);                                
                temp.isActive = false;                
            }
        }
    }

    private void resetEventFromPhase(AIPhase phase)
    {
        foreach(AIEvent elem in this.events)
        {
            if (elem.affectedPhases.Contains(phase.phaseName) && elem.repeatEvent)
            {
                elem.isActive = true;
                elem.startTime = this.timeElapsed;
            }
        }
    }

    private bool isTriggered(AIEvent elem)
    {
        int success = 0;

        if (elem.isActive 
            && ((this.activePhase != null && elem.affectedPhases.Contains(this.activePhase.phaseName))
                || elem.affectedPhases.Count == 0))
        {
            foreach (AITrigger triggerElem in elem.trigger)
            {
                if (triggerElem.type == AIEventType.time && this.timeElapsed >= (triggerElem.time + elem.startTime))
                {
                    if (!elem.requireAll) return true;
                    else success++;
                }
                else if (triggerElem.type == AIEventType.life && this.enemy.life <= (this.enemy.stats.maxLife * triggerElem.life / 100))
                {
                    if (!elem.requireAll) return true;
                    else success++;
                }
                else if (triggerElem.type == AIEventType.range)
                {
                    foreach(RangeTriggered range in triggerElem.rangeTrigger)
                    {
                        if(range.isTriggered)
                        {
                            if (!elem.requireAll) return true;
                            else success++;
                        }
                    }                   
                }
            }

            if (elem.requireAll && success == elem.trigger.Count) return true;
        }

        return false;
    }

    public void resetAllEvents()
    {
        if(this.phases.Count > 0) this.activePhase = this.phases[0];
        this.AIstarted = false;

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

    private int setNextActionToDo(List<AIAction> actionList, int index, bool clearList)
    {
        AIAction action = actionList[index];

        if (action.type != AIActionType.dialog)
        {
            if (action.skillinstance == null && action.skill != null)
            {
                action.skillinstance = Utilities.Skill.setSkill(this.enemy, action.skill);
                action.skillinstance.showingIndicator = true;
            }

            this.activeAction = action;

            this.counter = action.amount;            
        }
        else
        {
            this.activeDialogAction = action;
        }

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
            else 
            {
                int currentAmountOfSameSkills = Utilities.Skill.getAmountOfSameSkills(skill, this.enemy.activeSkills, this.enemy.activePets);

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
        if (this.activeAction != null) this.enemy.resetCast(this.activeAction.skillinstance);
        this.activeAction = null;
        this.actionsIndex = 0;
        this.counter = 0;
        this.globalCoolDown = 0;
    }

    private void useAction(AIAction action)
    {
        bool actionUsed = false;

        if (action.type == AIActionType.skill && skillCanBeUsed(action.skillinstance))
        {
            //useskill
            StandardSkill usedSkill = Utilities.Skill.instantiateSkill(action.skillinstance, this.enemy, this.enemy.target);
            action.skillinstance.target = null;
            action.skillinstance.cooldownTimeLeft = action.skillinstance.cooldown;
            actionUsed = true;
            //Debug.Log("Using action: " + usedSkill.skillName);
        }
        else if (action.type == AIActionType.movement)
        {
            //move enemy to position    
            //actionUsed = true;
        }
        else if (action.type == AIActionType.sequence)
        {
            //TODO: Utilities
            SkillSequence sequence = Instantiate(action.sequence);
            sequence.setSender(this.enemy);
            sequence.setTarget(this.enemy.target);
            actionUsed = true;
        }
        else if (action.type == AIActionType.transition)
        {
            //switch phase

            clearAction();
            this.activePhase = getPhaseByName(action.nextPhase);
            resetEventFromPhase(this.activePhase);
            //Debug.Log("Phase changed to: " + action.nextPhase);
            //actionUsed = true;
        }
        else if (action.type == AIActionType.wait)
        {
            //wait
            //Debug.Log("Wait "+action.gcd+" seconds");
            StartCoroutine(stopCo(action.gcd));
        }
        else if (action.type == AIActionType.kill)
        {
            //suicide
            this.enemy.KillIt();
        }
        else if (action.type == AIActionType.animation)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.enemy.animator, action.animation);
        }
        else if (action.type == AIActionType.immortal)
        {
            //suicide
            this.enemy.setImmortal(action.dauer);
        }

        if (actionUsed)
        {
            this.counter--;
        }

        if ((this.activeAction != null && !this.activeAction.castGroup) || this.counter <= 0)
        {
            if (this.activeAction != null) this.enemy.resetCast(this.activeAction.skillinstance);
            this.counter = 0;            

            //Debug.Log("Set GCD: " + action.gcd);
            this.globalCoolDown = action.gcd;
            this.activeAction = null;
        }
    }

    private AIPhase getPhaseByName(string value)
    {
        foreach (AIPhase phase in this.phases)
        {
            if (phase.phaseName.ToUpper() == value.ToUpper())
            {
                return phase;
            }
        }

        return null;
    }


    #endregion

}

