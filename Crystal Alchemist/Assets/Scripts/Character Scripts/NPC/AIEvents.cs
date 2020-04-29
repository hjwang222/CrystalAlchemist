using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

    private MiniDialogBox activeDialog;
    private AIPhase activePhase;
    private bool isActive;
    #endregion

    //BUG: To many events
    //BUG: Gifthaut doppelt
    private void Start()
    {
        InitializeTargeting(this.npc);
    }

    private void OnEnable()
    {
        if (this.startPhase != null && this.startImmediately) StartPhase();
    }

    public void StartPhase()
    {
        StartPhase(this.startPhase);
    }

    private void Update()
    {
        if (this.activePhase != null && !StatusEffectUtil.isCharacterStunned(this.npc))
            this.activePhase.Updating(this.npc);
    }

    private void OnDisable()
    {
        EndPhase();
    }

    private void OnDestroy()
    {
        EndPhase();
    }

    public void EndPhase()
    {
        this.isActive = false;
        if(this.activePhase != null) Destroy(this.activePhase);
    }

    public void StartPhase(AIPhase phase)
    {
        if (phase != null)
        {
            this.isActive = true;

            if (this.activePhase != null)
            {
                this.activePhase.ResetActions();
                Destroy(this.activePhase);
            }
            this.activePhase = Instantiate(phase);
            this.activePhase.Initialize();
        }
    }

    public void ShowDialog(string text, float duration)
    {
        if (this.activeDialog == null)
        {
            this.activeDialog = Instantiate(MasterManager.miniDialogBox, this.npc.transform);
            this.activeDialog.setDialogBox(text, duration);
        }
    }

}

