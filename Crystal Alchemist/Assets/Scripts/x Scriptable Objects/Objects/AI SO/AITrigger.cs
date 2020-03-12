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

    [ShowIf("type", AITriggerType.range)]
    [SerializeField]
    private List<RangeTriggered> rangeTrigger;

    private bool timesUp = false;

    public void Start()
    {
        ExternalCoroutine.instance.StartCoroutine(this.countDownCO());
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
        foreach (RangeTriggered range in this.rangeTrigger)
        {
            if (range.isTriggered) return true;
        }
        return false;
    }

    private IEnumerator countDownCO()
    {
        this.timesUp = false;
        yield return new WaitForSeconds(this.time);
        this.timesUp = true;
    }
}
