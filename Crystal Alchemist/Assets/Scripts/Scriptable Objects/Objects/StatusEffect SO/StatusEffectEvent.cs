using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class StatusEffectEvent
{
    [SerializeField]
    [BoxGroup("Trigger")]
    private StatusEffectTrigger trigger;

    [SerializeField]
    private List<StatusEffectAction> actions;

    private float elapsed;

    public void Initialize(Character character, StatusEffect effect)
    {
        if (this.trigger.triggerType == StatusEffectTriggerType.init) DoActions(character, effect);
    }

    public void Updating(float timeDistortion)
    {
        this.elapsed += (Time.deltaTime * timeDistortion);
    }

    private void DoActions(Character character, StatusEffect effect)
    {
        foreach (StatusEffectAction action in this.actions)
        {
            action.DoAction(character, effect);
        }
    }    

    public void DoEvents(Character character, StatusEffect effect)
    {
        if (isTriggered(character)) DoActions(character, effect);      
    }

    private bool isTriggered(Character character)
    {
        bool isTriggered = false;

        switch (this.trigger.triggerType)
            {
                case StatusEffectTriggerType.intervall: if (this.elapsed >= trigger.intervall) { isTriggered = true; this.elapsed = 0; } break;
                case StatusEffectTriggerType.life: if (character != null && character.values.life <= trigger.life) isTriggered = true; break;
                case StatusEffectTriggerType.mana: if (character != null && character.values.mana <= trigger.mana) isTriggered = true; break;
            }        

        return isTriggered;
    }

    public void ResetEvent(Character character, StatusEffect effect)
    {
        if (this.trigger.triggerType == StatusEffectTriggerType.destroyed) DoActions(character, effect);

        foreach (StatusEffectAction action in this.actions)
        {
            action.ResetAction(character);
        }
    }
}
