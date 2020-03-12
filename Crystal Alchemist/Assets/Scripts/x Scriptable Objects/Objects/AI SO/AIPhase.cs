using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "AI/AI Phase")]
public class AIPhase : ScriptableObject
{
    [BoxGroup("Action Sequence")]
    [SerializeField]
    public List<AIAction> actions;

    [BoxGroup("Triggered Events")]
    [SerializeField]
    public List<AIEvent> events;

    public void Start()
    {
        foreach (AIEvent aiEvent in this.events)
        {
            aiEvent.Start();
        }
    }
}
