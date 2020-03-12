using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public enum RequirementType
{
    single,
    all
}

[CreateAssetMenu(menuName = "AI/AI Event")]
public class AIEvent : ScriptableObject
{
    [SerializeField]
    private List<AITrigger> triggers;

    [SerializeField]
    private List<AIAction> actions;

    [SerializeField]
    private bool repeatEvent = false;

    [SerializeField]
    [ShowIf("repeatEvent")]
    private float eventCooldown = 0;

    [SerializeField]
    [EnumToggleButtons]
    private RequirementType type;

    private bool eventActive = true;
    private float timeLeft = 0;

    public void Start(List<RangeTriggered> ranges)
    {
        foreach (AITrigger trigger in this.triggers)
        {
            trigger.Start(ranges);
        }
    }

    public void Update(AI npc)
    {
        foreach(AITrigger trigger in this.triggers)
        {
            trigger.Update();
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

    public bool eventEnabled(AI npc)
    {
        if (this.eventActive && this.isTriggered(npc)) return true;
        return false;
    }

    public List<AIAction> getActions()
    {
        this.eventActive = false;
        this.timeLeft = this.eventCooldown;
        return this.actions;
    }


}