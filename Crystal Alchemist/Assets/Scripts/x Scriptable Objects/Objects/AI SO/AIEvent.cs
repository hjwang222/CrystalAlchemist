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
    private List<AITrigger> triggers;

    [SerializeField]
    private List<AIAction> actions;

    [SerializeField]
    private bool repeatEvent = false;

    [SerializeField]
    private bool startImmediately = false;

    [SerializeField]
    [ShowIf("repeatEvent")]
    private float eventCooldown = 0;

    [SerializeField]
    [EnumToggleButtons]
    private RequirementType type;

    private bool eventActive = true;
    private float timeLeft = 0;

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

        if ((this.type == RequirementType.all && triggerCount == this.triggers.Count)
         || (this.type == RequirementType.single && triggerCount > 0)) return true;

        return false;
    }

    public void SetEventActions(AI npc, List<AIAction> actions, AIPhase phase)
    {
        if (this.eventActive && this.isTriggered(npc))
        {
            this.eventActive = false;
            if(this.repeatEvent) this.timeLeft = this.eventCooldown;
            actions.AddRange(this.actions);
            if (this.startImmediately) phase.ResetActions();
        }
    }
}