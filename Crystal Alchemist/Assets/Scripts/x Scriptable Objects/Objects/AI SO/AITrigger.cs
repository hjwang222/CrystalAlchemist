using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public enum AITriggerType
{
    time,
    life,
    range
}

[CreateAssetMenu(menuName = "AI/AI Trigger")]
public class AITrigger : ScriptableObject
{
    [SerializeField]
    private AITriggerType type;

    [ShowIf("type", AITriggerType.time)]
    [SerializeField]
    private float time;

    [ShowIf("type", AITriggerType.life)]
    [SerializeField]
    private float life;

    private List<RangeTriggered> rangeTrigger;
    private bool timesUp = false;
    private float elapsed = 0;

    public void Initialize(List<RangeTriggered> ranges)
    {
        this.rangeTrigger = ranges;
        startTimer();
    }

    public void Updating()
    {
        if (this.time > 0)
        {
            if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
            else { this.elapsed = 0; this.timesUp = true; }
        }
    }

    public bool isTriggered(AI npc)
    {
        if (this.type == AITriggerType.life) return checkLife(npc);
        else if (this.type == AITriggerType.range) return checkRange();
        else if (this.type == AITriggerType.time && this.timesUp) return true;
        return false;
    }

    private bool checkLife(AI npc)
    {
        return (npc.life <= (npc.maxLife * this.life / 100));
    }

    private bool checkRange()
    {
        if (this.rangeTrigger != null)
        {
            foreach (RangeTriggered range in this.rangeTrigger)
            {
                if (range.isTriggered) return true;
            }
        }
        return false;
    }

    private void startTimer()
    {
        this.elapsed = this.time;
        this.timesUp = false;
    }
}
