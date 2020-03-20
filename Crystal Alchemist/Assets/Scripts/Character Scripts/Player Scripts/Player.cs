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
    public StringValue saveGameSlot;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public PlayerInventory playerInventory;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public FloatValue fadingDuration;

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

        LoadSystem.loadPlayerData(this, this.saveGameSlot.getValue());
        AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", false);

        this.characterLookDown();

        updateResource(ResourceType.life, 0);
        updateResource(ResourceType.mana, 0);
    }

    public override void prepareSpawnOut()
    {
        base.prepareSpawnOut();
        this.deactivateAllSkills();
        this.fadeSignal.Raise(false);
    }

    

    private void deactivateAllSkills()
    {
        for (int i = 0; i < this.activeSkills.Count; i++)
        {
            Skill activeSkill = this.activeSkills[i];
            activeSkill.DeactivateIt();
        }
    }

    public override void KillIt()
    {
        if (this.currentState != CharacterState.dead)
        {
            this.change = Vector2.zero;
            this.characterLookDown();

            //TODO: Kill sofort (Skill noch aktiv)
            StatusEffectUtil.RemoveAllStatusEffects(this.debuffs);
            StatusEffectUtil.RemoveAllStatusEffects(this.buffs);

            //this.spriteRenderer.color = Color.white;
            AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);

            this.currentState = CharacterState.dead;
            this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            this.deathSignal.Raise();
        }
    }

    ///////////////////////////////////////////////////////////////


    public override bool HasEnoughCurrency(Costs price)
    {
        bool result = false;

        if (price.resourceType == ResourceType.none) result = true;
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
        if (price.resourceType == ResourceType.life) return this.life;
        else if (price.resourceType == ResourceType.mana) return this.mana;
        else if (price.resourceType == ResourceType.item && price.item != null) return this.GetComponent<PlayerItems>().GetAmount(price.item);
        return 0;
    }



    public override void reduceResource(Costs price)
    {
        //Shop, Door, Treasure, MiniGame, Abilities, etc
        if ((price.item != null && !price.item.isKeyItem) || price.item == null)
            this.updateResource(price.resourceType, price.item, -price.amount);
    }



    ///////////////////////////////////////////////////////////////
       

    public override void updateResource(ResourceType type, ItemGroup item, float value, bool showingDamageNumber)
    {
        base.updateResource(type, null, value, showingDamageNumber);

        switch (type)
        {
            case ResourceType.life: callSignal(this.healthSignalUI, value); break;
            case ResourceType.mana: callSignal(this.manaSignalUI, value); break;
            case ResourceType.item: this.GetComponent<PlayerItems>().UpdateInventory(item, Mathf.RoundToInt(value)); break;
        }
    }

    #region Menu und DialogBox

    public void setStateMenuOpened(CharacterState newState)
    {
        StopCoroutine(delayInputPlayerCO(GlobalValues.playerDelay, newState));
        this.currentState = newState;
    }

    public void setStateAfterMenuClose(CharacterState newState)
    {
        StartCoroutine(delayInputPlayerCO(GlobalValues.playerDelay, newState));
    }

    public void showDialogBox(string text)
    {
        if (this.currentState != CharacterState.inDialog) this.dialogBoxSignal.Raise(text);
    }

    public IEnumerator delayInputPlayerCO(float delay, CharacterState newState)
    {
        //Damit der Spieler nicht gleich wieder die DialogBox aktiviert : /
        yield return new WaitForSeconds(delay);
        this.currentState = newState;
    }

    #endregion
}
