using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class Player : Character
{
    [BoxGroup("Pflichtfelder")]
    [Required]
    public TimeValue timeValue;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public FloatValue secondsPlayed;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public PlayerStats playerStats;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public CharacterPreset preset;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public CharacterPreset defaultPreset;

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

    [Required]
    [BoxGroup("Pflichtfelder")]
    public StringValue saveGameSlot;

    ///////////////////////////////////////////////////////////////

    public override void Awake()
    {
        this.playerStats.player = this;
        this.Initialize();
        if (this.playerStats.loadPlayer) LoadSystem.loadPlayerData(this, this.saveGameSlot.getValue());

        this.GetComponent<PlayerTeleport>().Initialize(this); //start methods here
    }

    public override void Update()
    {
        base.Update();

        //Update methods here
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
        bool result = false;

        if (price.resourceType == CostType.none) result = true;
        else
        {
            if (this.getResource(price) - price.amount >= 0) result = true;
            else result = false;
        }

        return result;
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

    #region Menu und DialogBox

    public void setStateMenuOpened(CharacterState newState)
    {
        StopCoroutine(delayInputPlayerCO(MasterManager.staticValues.playerDelay, newState));
        this.values.currentState = newState;
    }

    public void setStateAfterMenuClose(CharacterState newState)
    {
        StartCoroutine(delayInputPlayerCO(MasterManager.staticValues.playerDelay, newState));
    }

    public void showDialogBox(string text)
    {
        if (this.values.currentState != CharacterState.inDialog) this.dialogBoxSignal.Raise(text);
    }

    public IEnumerator delayInputPlayerCO(float delay, CharacterState newState)
    {
        //Damit der Spieler nicht gleich wieder die DialogBox aktiviert : /
        yield return new WaitForSeconds(delay);
        this.values.currentState = newState;
    }

    #endregion


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

}



