using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class Player : Character
{
    [BoxGroup("Pflichtfelder")]
    [Required]
    public CharacterPreset defaultPreset;

    [HideInInspector]
    public CharacterPreset preset;

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
    public SimpleSignal buttonSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal presetSignal;

    ///////////////////////////////////////////////////////////////

    public override void Awake() => this.Initialize();
    
    private void Start()
    {
        //LoadSystem.loadPlayerData(this, this.saveGameSlot.getValue()); //load data from savegame once

        this.GetComponent<PlayerAbilities>().Initialize(this);
        this.GetComponent<PlayerTeleport>().Initialize(this);
        //this.playerStats.player = this;
    }


    public override void Update()
    {
        base.Update();
        this.GetComponent<PlayerAbilities>().Updating();
    }


    public override void SpawnOut()
    {
        base.SpawnOut();
        this.deactivateAllSkills();
    }

    public bool canUseAbilities()
    {
        if (this.values.currentState != CharacterState.interact
         && this.ActiveInField()) return true;
        return false;
    }


    private void deactivateAllSkills()
    {
        for (int i = 0; i < this.values.activeSkills.Count; i++)
        {
            Skill activeSkill = this.values.activeSkills[i];
            activeSkill.DeactivateIt();
        }
    }

    public override void Dead()
    {
        if (this.values.currentState != CharacterState.dead)
        {
            this.SetDefaultDirection();

            StatusEffectUtil.RemoveAllStatusEffects(this.values.debuffs);
            StatusEffectUtil.RemoveAllStatusEffects(this.values.buffs);
            AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);

            this.values.currentState = CharacterState.dead;
            this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            this.deathSignal.Raise();
        }
    }

    ///////////////////////////////////////////////////////////////


    public override bool HasEnoughCurrency(Costs price)
    {
        if (price.resourceType == CostType.none) return true;
        else if (this.getResource(price) - price.amount >= 0) return true;  
        return false;
    }

    public float getResource(Costs price)
    {
        //Buttons und hasEnoughCurrency
        if (price.resourceType == CostType.life) return this.values.life;
        else if (price.resourceType == CostType.mana) return this.values.mana;
        else if (price.resourceType == CostType.item && price.item != null) return this.GetComponent<PlayerItems>().GetAmount(price.item);
        return 0;
    }

    public override void reduceResource(Costs price)
    {
        //Shop, Door, Treasure, MiniGame, Abilities, etc
        if (price != null
            && ((price.item != null && !price.item.isKeyItem())
              || price.item == null))
            this.updateResource(price.resourceType, price.item, -price.amount);
    }



    ///////////////////////////////////////////////////////////////


    public override void updateResource(CostType type, ItemGroup item, float value, bool showingDamageNumber)
    {
        base.updateResource(type, null, value, showingDamageNumber);

        switch (type)
        {
            case CostType.life: callSignal(this.healthSignalUI, value); break;
            case CostType.mana: callSignal(this.manaSignalUI, value); break;
            case CostType.item: this.GetComponent<PlayerItems>().UpdateInventory(item, Mathf.RoundToInt(value)); break;
        }
    }

    public void callSignal(SimpleSignal signal, float addResource)
    {
        if (signal != null && addResource != 0) signal.Raise();
    }


    public bool CanMove()
    {
        return (CanOpenMenu() && !StatusEffectUtil.isCharacterStunned(this));
    }

    public bool CanOpenMenu()
    {
        return (this.values.currentState != CharacterState.inDialog
             && this.values.currentState != CharacterState.inMenu
             && this.values.currentState != CharacterState.respawning
             && this.values.currentState != CharacterState.dead);
    }

    public void showDialogBox(string text)
    {
        if (this.values.currentState != CharacterState.inDialog) this.dialogBoxSignal.Raise(text);
    }

}



