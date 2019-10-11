using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



public class Player : Character
{
    [FoldoutGroup("Skills", expanded: false)]
    [Tooltip("Skills, welcher der Character verwenden kann")]
    public List<Skill> skillSet = new List<Skill>();

    [BoxGroup("Pflichtfelder")]
    [Required]
    public TimeValue timeValue;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public PlayerStats playerStats;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public StringSignal dialogBoxSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal deathSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal healthSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal manaSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal openInventorySignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal openPauseSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public BoolSignal fadeSignal;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private GameObject targetHelpObject;

    [Required]
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private BoolValue loadGame;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public FloatValue fadingDuration;

    [HideInInspector]
    public Skill AButton;
    [HideInInspector]
    public Skill BButton;
    [HideInInspector]
    public Skill XButton;
    [HideInInspector]
    public Skill YButton;
    [HideInInspector]
    public Skill RBButton;

    [HideInInspector]
    public Vector3 change;


    ///////////////////////////////////////////////////////////////

    private void Awake()
    {
        initPlayer();
    }

    public void initPlayer()
    {
        this.playerStats.player = this;

        SaveSystem.loadOptions();

        this.isPlayer = true;
        this.init();

        if (this.loadGame.getValue()) LoadSystem.loadPlayerData(this);

        if (this.targetHelpObject != null) setTargetHelper(this.targetHelpObject);
        Utilities.Helper.checkIfHelperDeactivate(this);

        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Dead", false);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", 0);
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", -1);

        this.direction = new Vector2(0, -1);
        //this.currencySignalUI.Raise();
    }

    public void delay(CharacterState newState)
    {
        StartCoroutine(Utilities.Skills.delayInputPlayerCO(GlobalValues.playerDelay, this, newState));
    }

    public void showDialogBox(string text)
    {
        if (this.currentState != CharacterState.inDialog) this.dialogBoxSignal.Raise(text);
    }

    public override void KillIt()
    {
        if (this.currentState != CharacterState.dead)
        {
            this.change = Vector2.zero;
            this.direction = new Vector2(0, -1);

            //TODO: Kill sofort (Skill noch aktiv)
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.debuffs);
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.buffs);

            this.spriteRenderer.color = Color.white;

            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", 0);
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", -1);
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Dead", true);

            this.currentState = CharacterState.dead;
            this.deathSignal.Raise();
        }
    }

    ///////////////////////////////////////////////////////////////

    public override void updateResource(ResourceType type, Item item, float value, bool showingDamageNumber)
    {
        base.updateResource(type, item, value, showingDamageNumber);

        switch (type)
        {
            case ResourceType.life:
                {
                    callSignal(this.healthSignalUI, value); break;
                }
            case ResourceType.mana:
                {
                    callSignal(this.manaSignalUI, value); break;
                }
        }
    }








}
