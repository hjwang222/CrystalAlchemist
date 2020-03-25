using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public enum RequirementType
{
    single,
    all
}

[System.Serializable]
public class AIEvent
{
    [SerializeField]
    private List<AITrigger> triggers = new List<AITrigger>();

    [SerializeField]
    private List<AIAction> actions = new List<AIAction>();

    [SerializeField]
    private bool repeatEvent = false;

    [SerializeField]
    private bool interruptCurrentAction = false;

    [SerializeField]
    [ShowIf("repeatEvent")]
    private float eventCooldown = 0;

    [SerializeField]
    private RequirementType requirementsNeeded = RequirementType.single;

    private bool eventActive = true;
    private float timeLeft = 0;


    public void Initialize()
    {
        this.eventActive = true;
        this.timeLeft = 0;

        foreach (AITrigger trigger in this.triggers)
        {
            trigger.Initialize();
        }
    }

    public void Updating(AI npc)
    {
        if (this.eventActive)
        {
            foreach (AITrigger trigger in this.triggers)
            {
                trigger.Updating();
            }
        }

        if (!this.eventActive && this.repeatEvent) updateTimer();              
    }

    private void updateTimer()
    {
        if (this.timeLeft > 0) this.timeLeft -= Time.deltaTime;
        else { this.timeLeft = 0; this.eventActive = true; }
    }

    private bool isTriggered(AI npc)
    {
        int triggerCount = 0;

        foreach (AITrigger trigger in this.triggers)
        {
            if (trigger.isTriggered(npc)) triggerCount++;
        }

        if ((this.requirementsNeeded == RequirementType.all && triggerCount == this.triggers.Count)
         || (this.requirementsNeeded == RequirementType.single && triggerCount > 0)) return true;

        return false;
    }

    public void SetEventActions(AI npc, List<AIAction> actions, AIPhase phase)
    {
        if (this.eventActive && this.isTriggered(npc))
        {
            this.eventActive = false;
            if(this.repeatEvent) this.timeLeft = this.eventCooldown;

            foreach(AIAction eventAction in this.actions)
            {
                if (!actions.Contains(eventAction)) actions.Add(eventAction);
            }

            if (this.interruptCurrentAction) phase.ResetActions();
        }
    }
}