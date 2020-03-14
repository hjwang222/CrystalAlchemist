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

    [BoxGroup("Events")]
    [SerializeField]
    private List<AIEvent> events;

    private AIAction currentAction;
    private AIAction currentDialog;
    private AIAction currentEventAction;
    private List<AIAction> eventActions = new List<AIAction>();
    private int index;
    private int eventIndex;


    public void Initialize()
    {
        foreach (AIEvent aiEvent in this.events)
        {
            aiEvent.Initialize();
        }

        ResetActions();
    }

    public void Updating(AI npc)
    {
        if (this.currentAction != null && !this.currentAction.getActive()) this.currentAction = null;
        if (this.currentDialog != null && !this.currentDialog.getActive()) this.currentDialog = null;

        foreach(AIEvent aiEvent in this.events)
        {
            aiEvent.Updating(npc);
            aiEvent.SetEventActions(npc, this.eventActions, this);
        }

        SetNextAction(npc);
        UpdatingAction(npc);
    }

    private void SetNextAction(AI npc)
    {
        if(this.eventActions.Count == 0)
        {
            if (this.index < this.actions.Count)
            {
                if (this.currentDialog == null && this.actions[this.index].isDialog())
                {
                    this.currentDialog = this.actions[this.index];
                    this.currentDialog.Initialize(npc);
                    this.index++;
                }
                else if (this.currentAction == null && !this.actions[this.index].isDialog())
                {
                    this.currentAction = this.actions[this.index];
                    this.currentAction.Initialize(npc);
                    this.index++;
                }
            }
            else
            {
                if (this.currentAction == null
                    && this.currentDialog == null
                    && this.repeat)
                    this.index = 0;                
            }
        }
        else
        {
            if (this.eventIndex < this.eventActions.Count)
            {
                if (this.currentDialog == null && this.eventActions[this.eventIndex].isDialog())
                {
                    this.currentDialog = this.eventActions[this.eventIndex];
                    this.currentDialog.Initialize(npc);
                    this.eventIndex++;
                }
                else if (this.currentAction == null && !this.eventActions[this.eventIndex].isDialog())
                {
                    this.currentAction = this.eventActions[this.eventIndex];
                    this.currentAction.Initialize(npc);
                    this.eventIndex++;
                }
            }
            else
            {
                this.eventIndex = 0;
                this.eventActions.Clear();
            }
        }
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
