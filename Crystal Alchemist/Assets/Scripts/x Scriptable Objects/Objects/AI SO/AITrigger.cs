using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public enum AITriggerType
{
    time,
    life,
    range, 
    aggro,
    aggroLost
}

[System.Serializable]
public class AITrigger
{
    [SerializeField]
    private AITriggerType type;

    [ShowIf("type", AITriggerType.time)]
    [SerializeField]
    private float time;

    [ShowIf("type", AITriggerType.life)]
    [SerializeField]
    private float life;

    [ShowIf("type", AITriggerType.range)]
    [SerializeField]
    private List<RangeTriggered> rangeTrigger;

    private bool timesUp = false;
    private float elapsed = 0;

    public void Initialize()
    {
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
        if (this.type == AITriggerType.aggro) return checkAggro(npc);
        else if (this.type == AITriggerType.aggroLost) return checkAggroLost(npc);
        else if (this.type == AITriggerType.life) return checkLife(npc);
        else if (this.type == AITriggerType.range) return checkRange();
        else if (this.type == AITriggerType.time) return checkTime();
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

    private bool checkAggro(AI npc)
    {
        return (npc.target != null);
    }

    private bool checkAggroLost(AI npc)
    {
        return (npc.target == null);
    }

    private bool checkTime()
    {
        return this.timesUp;
    }


    private void startTimer()
    {
        this.elapsed = this.time;
        this.timesUp = false;
    }
}
