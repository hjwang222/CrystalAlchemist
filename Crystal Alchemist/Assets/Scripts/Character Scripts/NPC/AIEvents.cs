using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class AIEvents : CharacterCombat
{
    #region Attributes

    [SerializeField]
    [Required]
    [BoxGroup("Required")]
    private AI npc;

    [BoxGroup("AI")]
    [SerializeField]
    private AIPhase startPhase;

    [BoxGroup("AI")]
    [SerializeField]
    [Tooltip("False, wenn Animator Event verwendet wird")]
    private bool startImmediately = true;

    private AIPhase activePhase;
    private bool isActive;
    #endregion

    private void Start() => InitializeTargeting(this.npc);    

    private void OnEnable()
    {
        if (this.startPhase != null && this.startImmediately) StartPhase();
    }

    public void StartPhase() => StartPhase(this.startPhase);    

    private void Update()
    {
        if (this.activePhase != null && !this.npc.values.isCharacterStunned())
            this.activePhase.Updating(this.npc);
    }

    private void OnDisable() => EndPhase();    

    private void OnDestroy() => EndPhase();    

    public void EndPhase()
    {
        this.isActive = false;
        DestroyActivePhase();        
    }

    private void DestroyActivePhase()
    {
        if (this.activePhase != null)
        {
            this.activePhase.ResetActions(this.npc);
            Destroy(this.activePhase);
        }
    }

    public void StartPhase(AIPhase phase)
    {
        if (phase != null)
        {
            this.isActive = true;

            DestroyActivePhase();
            this.activePhase = Instantiate(phase);
            this.activePhase.Initialize(this.npc);
        }
    }

    public override List<Character> GetTargetsFromTargeting()
    {
        List<Character> result = new List<Character>();
        foreach(KeyValuePair<Character, float[]> aggro in this.npc.aggroList) result.Add(aggro.Key);
        return result;
    }

    public override void ShowTargetingSystem(Ability ability)
    {
        
    }
}

