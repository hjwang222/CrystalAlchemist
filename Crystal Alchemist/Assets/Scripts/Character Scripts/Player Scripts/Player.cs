using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Required]
    [BoxGroup("Player Objects")]
    [SerializeField]
    private SimpleSignal healthSignalUI;

    [Required]
    [BoxGroup("Player Objects")]
    [SerializeField]
    private SimpleSignal manaSignalUI;

    [Required]
    [BoxGroup("Player Objects")]
    [SerializeField]
    private SimpleSignal presetSignal;

    [Required]
    [BoxGroup("Player Objects")]
    [SerializeField]
    private StringValue characterName;

    [BoxGroup("Player Objects")]
    [SerializeField]
    private float goToBedDuration = 1f;

    [BoxGroup("Player Objects")]
    [SerializeField]
    private BoolValue CutSceneValue;

    ///////////////////////////////////////////////////////////////

    public override void Awake()
    {        
        this.values.Initialize();    
        SetComponents();
    }

    public override void OnEnable()
    {
        if (this.values.life <= 0) ResetValues();
    }

    public override void Start()
    {
        base.Start();
        this.presetSignal.Raise();

        GameEvents.current.OnCollect += this.CollectIt;
        GameEvents.current.OnReduce += this.reduceResource;
        GameEvents.current.OnStateChanged += this.SetState;
        GameEvents.current.OnSleep += this.GoToSleep;
        GameEvents.current.OnWakeUp += this.WakeUp;
        GameEvents.current.OnCutScene += this.SetCutScene;
        GameEvents.current.OnEnoughCurrency += this.HasEnoughCurrency;

        if( this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Initialize();
        PlayerComponent[] components = this.GetComponents<PlayerComponent>();
        for (int i = 0; i < components.Length; i++) components[i].Initialize();

        this.healthSignalUI.Raise();
        this.manaSignalUI.Raise();    
        this.ChangeDirection(this.values.direction);

        this.animator.speed = 1;
        this.updateTimeDistortion(0);
        this.AddStatusEffectVisuals();
    }       

    public override void Update()
    {
        base.Update();        
        if(this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Updating();
        PlayerComponent[] components = this.GetComponents<PlayerComponent>();
        for (int i = 0; i < components.Length; i++) components[i].Updating();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameEvents.current.OnCollect -= this.CollectIt;
        GameEvents.current.OnReduce -= this.reduceResource;
        GameEvents.current.OnStateChanged -= this.SetState;
        GameEvents.current.OnSleep -= this.GoToSleep;
        GameEvents.current.OnWakeUp -= this.WakeUp;
        GameEvents.current.OnCutScene -= this.SetCutScene;
        GameEvents.current.OnEnoughCurrency -= this.HasEnoughCurrency;
    }

    public override void SpawnOut()
    {
        base.SpawnOut();        
        this.deactivateAllSkills();
    }

    public override void EnableScripts(bool value)
    {
        if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().enabled = value;
        //if (this.GetComponent<PlayerControls>() != null) this.GetComponent<PlayerControls>().enabled = value;
        if (this.GetComponent<PlayerMovement>() != null) this.GetComponent<PlayerMovement>().enabled = value;
        //if (this.GetComponent<PlayerInput>() != null) this.GetComponent<PlayerInput>().enabled = value;        
    }

    private void deactivateAllSkills()
    {
        for (int i = 0; i < this.values.activeSkills.Count; i++)
        {
            Skill activeSkill = this.values.activeSkills[i];
            activeSkill.DeactivateIt();
        }
    }

    public override void KillIt()
    {
        if (this.values.currentState != CharacterState.dead)
        {
            this.SetDefaultDirection();

            StatusEffectUtil.RemoveAllStatusEffects(this.values.debuffs);
            StatusEffectUtil.RemoveAllStatusEffects(this.values.buffs);
            AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);

            this.values.currentState = CharacterState.dead;
            this.myRigidbody.bodyType = RigidbodyType2D.Kinematic; //Static causes Room to empty
            MenuEvents.current.OpenDeath();
        }
    }

    ///////////////////////////////////////////////////////////////

    public override bool HasEnoughCurrency(Costs price)
    {
        if (price.resourceType == CostType.none) return true;
        else if (price.resourceType == CostType.life && this.values.life - price.amount >= 0) return true;
        else if (price.resourceType == CostType.mana && this.values.mana - price.amount >= 0) return true;
        else if (price.resourceType == CostType.keyItem && price.keyItem != null && GameEvents.current.HasKeyItem(price.keyItem.name)) return true;
        else if (price.resourceType == CostType.item && price.item != null && GameEvents.current.GetItemAmount(price.item) - price.amount >= 0) return true;

        return false;
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
        UpdateLifeMana(type, null, value, showingDamageNumber);

        switch (type)
        {
            case CostType.life: callSignal(this.healthSignalUI, value); break;
            case CostType.mana: callSignal(this.manaSignalUI, value); break;
            case CostType.item: this.GetComponent<PlayerItems>().UpdateInventory(item, Mathf.RoundToInt(value)); break;
        }

        CheckDeath();
    }

    private void SetCutScene()
    {
        if (this.CutSceneValue.GetValue()) this.values.currentState = CharacterState.respawning;
        else this.values.currentState = CharacterState.idle;
    }

    public void callSignal(SimpleSignal signal, float addResource)
    {
        if (signal != null && addResource != 0) signal.Raise();
    }


    private void SetState(CharacterState state) => this.values.currentState = state;
    






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

    public override string GetCharacterName()
    {
        return this.characterName.GetValue();
    }

    /////////////////////////////////////////////////////////////////////////////////
    

    public void GoToSleep(Vector2 position, Action before, Action after)
    {
        StartCoroutine(GoToBed(goToBedDuration, position, before, after));
    }

    public void WakeUp(Vector2 position, Action before, Action after)
    {
        StartCoroutine(GetUp(goToBedDuration, position, before, after));
    }

    private IEnumerator GoToBed(float duration, Vector2 position, Action before, Action after)
    {
        this.transform.position = position;
        yield return new WaitForEndOfFrame(); //Wait for Camera

        EnablePlayer(false); //Disable Movement and Collision

        before?.Invoke(); //Decke

        AnimatorUtil.SetAnimatorParameter(this.animator, "GoSleep");
        float animDuration = AnimatorUtil.GetAnimationLength(this.animator, "GoSleep");
        yield return new WaitForSeconds(animDuration);
        
        after?.Invoke(); //Zeit    
        this.boxCollider.enabled = true;
    }

    private void EnablePlayer(bool value)
    {
        this.EnableScripts(value); //prevent movement        
        this.boxCollider.enabled = value; //prevent input

        AnimatorUtil.SetAnimDirection(Vector2.down, this.animator);
    }

    private IEnumerator GetUp(float duration, Vector2 position, Action before, Action after)
    {
        this.boxCollider.enabled = false;
        before?.Invoke(); //Zeit    

        AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
        float animDuration = AnimatorUtil.GetAnimationLength(this.animator, "WakeUp");
        yield return new WaitForSeconds(animDuration);

        after?.Invoke(); //Decke

        EnablePlayer(true);

        this.transform.position = position;
    }
}