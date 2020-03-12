using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "AI/AI Phase")]
public class AIPhase : ScriptableObject
{
    [BoxGroup("Action Sequence")]
    [SerializeField]
    private List<AIAction> actions;

    [BoxGroup("Triggered Events")]
    [SerializeField]
    private List<AIEvent> events;

    private List<AIAction> currentActions = new List<AIAction>();
    private List<AIAction> currentDialog = new List<AIAction>();
    private List<AIAction> currentEvents = new List<AIAction>();

    private int index;

    public void Start(List<RangeTriggered> ranges)
    {       

        foreach (AIEvent aiEvent in this.events)
        {
            aiEvent.Start(ranges);
        }

        fillActions();
    }

    private void initializeActions()
    {
        List<AIAction> temp = new List<AIAction>();

        foreach (AIAction action in this.actions)
        {
            AIAction act = Instantiate(action);
            act.Start();
            temp.Add(act);
        }

        this.actions = temp;
    }

    public void fillActions()
    {
        this.currentActions.AddRange(this.actions);
    }

    public void Update(AI npc)
    {
        
    }

    public void useAction(AI npc)
    {
        if (this.currentActions.Count > 0)
        {
            this.currentActions[0].useAction(npc);
            this.currentActions.RemoveAt(0);
        }
    }
}
