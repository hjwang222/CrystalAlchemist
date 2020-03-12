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

    public void Start()
    {
        foreach (AITrigger trigger in this.triggers)
        {
            trigger.Start();
        }
    }

    public void Update(AI npc)
    {
        if (this.eventActive && this.isTriggered(npc)) this.doActions(npc);        
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

    private void doActions(AI npc)
    {
        foreach(AIAction action in this.actions)
        {
            action.useAction(npc);
        }

        ExternalCoroutine.instance.StartCoroutine(timerCO(this.eventCooldown));
    }

    private IEnumerator timerCO(float delay)
    {
        this.eventActive = false;
        yield return new WaitForSeconds(delay);
        if(this.repeatEvent) this.eventActive = true;
    }
}