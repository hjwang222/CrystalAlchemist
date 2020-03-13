using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(menuName = "AI/AI Phase")]
public class AIPhase : ScriptableObject
{
    [BoxGroup("Action Sequence")]
    [SerializeField]
    private List<AIAction> actions;

    [BoxGroup("Action Sequence")]
    [SerializeField]
    private bool repeat;



    private AIAction currentAction;
    private AIAction currentDialog;
    private AIAction currentEventAction;
    private List<AIAction> eventActions = new List<AIAction>();
    private List<AIEvent> events = new List<AIEvent>();
    private int index;
    private int eventIndex;


    public void Initialize(List<AIEvent> events)
    {
        ResetActions();
        this.events = events;
    }

    public void Updating(AI npc)
    {
        if (this.currentAction.getActive()) this.currentAction = null;
        if (this.currentDialog.getActive()) this.currentDialog = null;

        foreach(AIEvent aiEvent in this.events)
        {
            aiEvent.Updating(npc);
            aiEvent.SetEventActions(npc, this.eventActions, this);
        }

        if (this.eventActions.Count == 0) this.index = SetNextAction(npc, this.index, this.actions, false);
        else this.eventIndex = SetNextAction(npc, this.eventIndex, this.eventActions, true);

        UpdatingAction(npc);
    }

    private int SetNextAction(AI npc, int index, List<AIAction> actions, bool isEvent)
    {
        if (index < actions.Count)
        {
            if (this.currentDialog == null && actions[index].isDialog())
                SetActiveAction(this.currentDialog, actions[index], npc);

            else if (this.currentAction == null && !actions[index].isDialog())
                SetActiveAction(this.currentAction, actions[index], npc);

            if (index >= actions.Count)
            {
                if (this.repeat && !isEvent) index = 0;
                if (isEvent) { actions.Clear(); index = 0; }
            }            
        }
        return index;
    }

    private void SetActiveAction(AIAction current, AIAction action, AI npc)
    {        
        current = action;
        current.Initialize(npc);
        this.index++;
    }

    private void UpdatingAction(AI npc)
    {
        if (this.currentAction != null) this.currentAction.Updating(npc);
        if (this.currentDialog != null) this.currentDialog.Updating(npc);
    }

    public void ResetActions()
    {
        this.currentAction = null;
        this.currentDialog = null;

        //Destroy(this.currentAction);
        //Destroy(this.currentDialog);
    }
}
