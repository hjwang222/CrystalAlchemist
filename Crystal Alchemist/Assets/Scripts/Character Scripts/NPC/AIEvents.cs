using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AIEvents : MonoBehaviour
{
    #region Attributes

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI npc;

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

    [BoxGroup("AI")]
    [SerializeField]
    private List<AIPhase> phases = new List<AIPhase>();

    private CastBar activeCastBar;
    private MiniDialogBox activeDialog;
    private AIPhase activePhase;
    #endregion

    private void Start()
    {
        startPhase(this.phases[0]);
    }

    private void Update()
    {
        //this.activePhase.Update(this.npc);
    }

    private void startPhase(AIPhase phase)
    {
        this.activePhase = phase;
        this.activePhase.Start();

        foreach (AIEvent aiEvent in this.activePhase.events)
        {
            aiEvent.Start();
        }
    }

    private void UpdatePhase()
    {
        foreach(AIAction action in this.activePhase.actions)
        {
            foreach(AIEvent aiEvent in this.activePhase.events)
            {
                aiEvent.Update(this.npc);
            }

            action.useAction(this.npc);
        }
    }













    /*if (this.immortalAtStart > 0) this.setInvincible(this.immortalAtStart, false);

        AIEvents eventAI = this.GetComponent<AIEvents>();
        if (eventAI != null) eventAI.init();


    private void Start()
    {
        init();
    }

    private void OnDisable()
    {
        resetAllEvents();
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
                    action.skillinstance = CustomUtilities.Skills.setSkill(this.enemy, action.skill);                  
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

    private void updateCoolDownForSkills()
    {
        foreach (AIEvent AIevent in this.events)
        {
            foreach (AIAction action in AIevent.actions)
            {
                if (action.skillinstance != null && action.type == AIActionType.ability)
                {
                    //action.skillinstance.cooldownTimeLeft -= (Time.deltaTime * this.enemy.timeDistortion * this.enemy.spellspeed);
                }
            }
        }

        foreach (AIPhase phase in this.phases)
        {
            foreach (AIAction action in phase.actions)
            {
                if (action.skillinstance != null && action.type == AIActionType.ability)
                {
                    //action.skillinstance.cooldownTimeLeft -= (Time.deltaTime * this.enemy.timeDistortion * this.enemy.spellspeed);
                }
            }
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
        if (!CustomUtilities.StatusEffectUtil.isCharacterStunned(this.enemy))
        {
            this.timeElapsed += (Time.deltaTime * this.enemy.timeDistortion);

            //Event
            checkEvents();

            if (this.activeAction != null)
            {
                if (this.activeAction.skillinstance != null)
                {
                    /*
                    if (this.activeAction.cast >= 0) this.activeAction.skillinstance.cast = this.activeAction.cast;
                    if (this.activeAction.cD >= 0) this.activeAction.skillinstance.cooldown = this.activeAction.cD;   
                    
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
    if (this.activeAction.type == AIActionType.ability
     && this.activeAction.skillinstance != null)
    {
            if (this.enemy.target != null)
            {
                if (this.activeAction.skillinstance.target != this.enemy.target) this.activeAction.skillinstance.target = this.enemy.target;
                
                if (this.activeAction.skillinstance.holdTimer < this.activeAction.skillinstance.cast)
                {
                    this.activeAction.skillinstance.holdTimer += (Time.deltaTime * this.enemy.timeDistortion * this.enemy.spellspeed);

                    SkillIndicatorModule indicatorModule = this.activeAction.skillinstance.GetComponent<SkillIndicatorModule>();
                    if (indicatorModule != null) indicatorModule.showIndicator(); //Zeige Indikator beim Casten

                    SkillAnimationModule animationModule = this.activeAction.skillinstance.GetComponent<SkillAnimationModule>();
                    if (animationModule != null) animationModule.showCastingAnimation();

                    this.activeAction.skillinstance.doOnCast();

                    if (indicatorModule != null && indicatorModule.showCastBarForEnemies)
                    {
                        if (this.enemy.activeCastbar == null)
                        {
                            GameObject temp = Instantiate(this.castbar.gameObject, this.transform.position, Quaternion.identity, this.transform);
                            //temp.hideFlags = HideFlags.HideInHierarchy;
                            this.enemy.activeCastbar = temp.GetComponent<CastBar>();
                            //this.enemy.activeCastbar.target = this.enemy;
                            //this.enemy.activeCastbar.skill = this.activeAction.skillinstance;
                        }
                        else
                        {
                           //this.enemy.activeCastbar.showCastBar();
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

    GameObject dialog = Instantiate(this.box.gameObject, this.enemy.skillStartPosition.transform);

    MiniDialogBox temp = dialog.GetComponent<MiniDialogBox>();
    temp.setText(CustomUtilities.Format.getLanguageDialogText(action.de, action.en));
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
    foreach (AIEvent elem in this.events)
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
            if (triggerElem.type == AITriggerType.time && this.timeElapsed >= (triggerElem.time + elem.startTime))
            {
                if (!elem.requireAll) return true;
                else success++;
            }
            else if (triggerElem.type == AITriggerType.life && this.enemy.life <= (this.enemy.maxLife * triggerElem.life / 100))
            {
                if (!elem.requireAll) return true;
                else success++;
            }
            else if (triggerElem.type == AITriggerType.range)
            {
                foreach (RangeTriggered range in triggerElem.rangeTrigger)
                {
                    if (range.isTriggered)
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
    if (this.phases.Count > 0) this.activePhase = this.phases[0];
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
            action.skillinstance = CustomUtilities.Skills.setSkill(this.enemy, action.skill);
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

private bool skillCanBeUsed(Skill skill)
{
    if (skill != null)
    {
            if (skill.cooldownTimeLeft <= 0)            
            {
                int currentAmountOfSameSkills = CustomUtilities.Skills.getAmountOfSameSkills(skill, this.enemy.activeSkills, this.enemy.activePets);

                
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
}*/
}

