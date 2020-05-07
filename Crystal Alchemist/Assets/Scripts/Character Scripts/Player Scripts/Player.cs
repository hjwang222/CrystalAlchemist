using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class Player : Character
{
    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public StringSignal dialogBoxSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    public SimpleSignal deathSignal;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    [SerializeField]
    private SimpleSignal healthSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    [SerializeField]
    private SimpleSignal manaSignalUI;

    [Required]
    [FoldoutGroup("Player Signals", expanded: false)]
    [SerializeField]
    private SimpleSignal presetSignal;

    ///////////////////////////////////////////////////////////////

    public override void Awake()
    {
        SetComponents();
        this.values.Initialize();
        if (this.values.life <= 0) ResetValues();
    }
    
    public override void Start()
    {
        base.Start();
        this.presetSignal.Raise();

        GameEvents.current.OnCollect += this.CollectIt;
        GameEvents.current.OnReduce += this.reduceResource;
        GameEvents.current.OnMenuOpen += this.setStateMenuOpened;
        GameEvents.current.OnMenuClose += this.setStateAfterMenuClose;

        this.GetComponent<PlayerAbilities>().Initialize(this);
        this.GetComponent<PlayerTeleport>().Initialize(this);

        this.healthSignalUI.Raise();
        this.manaSignalUI.Raise();    
        this.ChangeDirection(this.values.direction);

        this.AddStatusEffectVisuals();
    }       

    public override void Update()
    {
        base.Update();        
        this.GetComponent<PlayerAbilities>().Updating();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameEvents.current.OnCollect -= this.CollectIt;
        GameEvents.current.OnReduce -= this.reduceResource;
        GameEvents.current.OnMenuOpen -= this.setStateMenuOpened;
        GameEvents.current.OnMenuClose -= this.setStateAfterMenuClose;
    }

    public override void SpawnOut()
    {
        base.SpawnOut();        
        this.deactivateAllSkills();
    }

    public override void SpawnIn()
    {     
        this.removeColor(Color.white);
        this.values.currentState = CharacterState.idle;
        this.EnableScripts(true);
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
        return (CanOpenMenu() && !this.values.isCharacterStunned());
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

    private void setStateMenuOpened(CharacterState newState)
    {
        StopCoroutine(delayInputPlayerCO(MasterManager.staticValues.playerDelay, newState));
        this.values.currentState = newState;
    }

    private void setStateAfterMenuClose(CharacterState newState)
    {
        StartCoroutine(delayInputPlayerCO(MasterManager.staticValues.playerDelay, newState));
        this.values.currentState = newState;
    }

    private IEnumerator delayInputPlayerCO(float delay, CharacterState newState)
    {
        //Damit der Spieler nicht gleich wieder die DialogBox aktiviert : /
        yield return new WaitForSeconds(delay);
        this.values.currentState = newState;
    }

    private void CollectIt(ItemStats stats)
    {
        //Collectable, Load, MiniGame, Shop und Treasure

        if (stats.resourceType == CostType.life || stats.resourceType == CostType.mana) updateResource(stats.resourceType, stats.amount, true);
        else if (stats.resourceType == CostType.item) GetComponent<PlayerItems>().CollectInventoryItem(stats);
        else if (stats.resourceType == CostType.none)
        {
            //if(this.ability != null)
            foreach (StatusEffect effect in stats.statusEffects)
            {
                StatusEffectUtil.AddStatusEffect(effect, this);
            }
        }
    }

}



