using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Character : MonoBehaviour
{
    [Required]
    [BoxGroup("Pflichtfelder")]
    public CharacterStats stats;

    #region Basic Attributes

    [Required]
    [BoxGroup("Easy Access")]
    public Rigidbody2D myRigidbody;

    [Required]
    [BoxGroup("Easy Access")]
    public Animator animator;

    [Required]
    [BoxGroup("Easy Access")]
    public Collider2D boxCollider;

    [BoxGroup("Easy Access")]
    [Required]
    [SerializeField]
    [Tooltip("Zur Erkennung wo der Charakter steht")]
    private SpriteRenderer shadowRenderer;

    [BoxGroup("Easy Access")]
    public RespawnAnimation respawnAnimation;

    [BoxGroup("Easy Access")]
    [Required]
    [SerializeField]
    [Tooltip("Position des Skills")]
    private GameObject skillStartPosition;

    [BoxGroup("Easy Access")]
    [Required]
    [SerializeField]
    [Tooltip("Position von Sprechblasen")]
    private GameObject headPosition;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject activeSkillParent;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject activeStatusEffectParent;

    #endregion

    #region Attributes

    private float regenTimeElapsed;
    private float manaTime;
    private DeathAnimation activeDeathAnimation;
    private bool cannotDie = false;
    private Vector3 spawnPosition;
    private CastBar activeCastbar;
    private List<StatusEffectGameObject> statusEffectVisuals = new List<StatusEffectGameObject>();

    [BoxGroup("Player")]
    public CharacterValues values;

    #endregion

    #region Start Functions (Spawn, Init)
    public virtual void Awake()
    {
        this.values = new CharacterValues(); //create new Values when not already assigned (NPC)
        this.values.Initialize();

        this.spawnPosition = this.transform.position;
        SetComponents();
    }

    public virtual void OnEnable()
    {
        this.transform.position = this.spawnPosition;
        ResetValues();
    }

    public virtual void Start() => GameEvents.current.OnEffectAdded += AddStatusEffectVisuals;

    public void SetComponents()
    {
        if (this.myRigidbody == null) this.myRigidbody = this.GetComponent<Rigidbody2D>();
        if (this.skillStartPosition == null) this.skillStartPosition = this.gameObject;
        if (this.animator == null) this.animator = this.GetComponent<Animator>();
        if (this.boxCollider == null) this.boxCollider = GetComponent<Collider2D>();
        if (this.boxCollider != null) this.boxCollider.gameObject.tag = this.transform.gameObject.tag;
    }

    public void ResetValues()
    {
        this.values.Clear(this.stats);

        this.SetDefaultDirection();
        this.animator.speed = 1;
        this.updateTimeDistortion(0);
        this.updateSpellSpeed(0);

        this.animator.enabled = true;
        this.SetCharacterSprites(true);
        this.activeDeathAnimation = null;

        if (this.stats.isMassive) this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
        else this.myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        if (this.GetComponent<CharacterRenderingHandler>() != null) this.GetComponent<CharacterRenderingHandler>().Reset();
        if (this.boxCollider != null) this.boxCollider.enabled = true;
    }

    public virtual void OnDestroy() => GameEvents.current.OnEffectAdded -= AddStatusEffectVisuals;

    #endregion

    #region Updates

    public virtual void Update()
    {
        if (this.values.currentState == CharacterState.dead
         || this.values.currentState == CharacterState.respawning) return;

        Regenerate();
        UpdateLifeAnimation();
        UpdateStatusEffects();
    }

    public void CheckDeath()
    {
        if (this.values.life <= 0
    && !this.cannotDie //Item
    && !this.values.isInvincible //Event
    && !this.values.cantBeHit) //after Hit
            Dead();
    }

    private void UpdateStatusEffects()
    {
        UpdateStatusEffectGroup(this.values.buffs);
        UpdateStatusEffectGroup(this.values.debuffs);
    }

    private void UpdateLifeAnimation()
    {
        float percentage = this.values.life * 100 / this.values.maxLife;
        AnimatorUtil.SetAnimatorParameter(this.animator, "Life", percentage);
    }

    private void Regenerate()
    {
        if (this.values.currentState != CharacterState.dead
            && this.values.currentState != CharacterState.respawning
            && this.stats.canRegenerate)
        {
            if (this.regenTimeElapsed >= this.stats.regenerationInterval)
            {
                this.regenTimeElapsed = 0;
                if (this.values.lifeRegen != 0 && this.values.life < this.values.maxLife) updateResource(CostType.life, this.values.lifeRegen);
                if (this.values.manaRegen != 0 && this.values.mana < this.values.maxMana) updateResource(CostType.mana, this.values.manaRegen, false);
            }
            else
            {
                this.regenTimeElapsed += (Time.deltaTime * this.values.timeDistortion);
            }
        }
    }

    #endregion

    #region Item Functions (drop Item, Lootregeln)

    public void dropItem()
    {
        if (this.values.itemDrop != null) this.values.itemDrop.Instantiate(this.transform.position);
    }

    #endregion

    #region Animation and Direction

    public void SetDefaultDirection() => ChangeDirection(new Vector2(0, -1));

    public void ChangeDirection(Vector2 direction)
    {
        this.values.direction = direction;
        AnimatorUtil.SetAnimDirection(direction, this.animator);
    }
    #endregion

    #region Color Changes

    public void removeColor(Color color)
    {
        if (this.GetComponent<CharacterRenderingHandler>() != null)
            this.GetComponent<CharacterRenderingHandler>().RemoveTint(color);
    }

    public void ChangeColor(Color color)
    {
        if (this.GetComponent<CharacterRenderingHandler>() != null)
            this.GetComponent<CharacterRenderingHandler>().ChangeTint(color);
    }

    public void setFlip()
    {
        if (this.GetComponent<CharacterRenderingHandler>() != null)
            this.GetComponent<CharacterRenderingHandler>().flipSprite(this.values.direction);
    }

    #endregion

    #region Update Resources

    public void updateSpeed(int addSpeed)
    {
        updateSpeed(addSpeed, true);
    }

    public void updateSpeed(int addSpeed, bool affectAnimation)
    {
        float startSpeedInPercent = this.stats.startSpeed / 100;
        float addNewSpeed = startSpeedInPercent * ((float)addSpeed / 100);
        float changeSpeed = startSpeedInPercent + addNewSpeed;

        this.values.speed = changeSpeed * this.values.timeDistortion * this.values.speedFactor;
        if (affectAnimation && this.stats.startSpeed > 0)
            this.animator.speed = this.values.speed / (this.stats.startSpeed * this.values.speedFactor / 100);
    }

    public void updateSpellSpeed(float addSpellSpeed)
    {
        this.values.spellspeed = ((this.stats.startSpellSpeed / 100) + (addSpellSpeed / 100)) * this.values.timeDistortion;
    }

    public void updateTimeDistortion(float distortion)
    {
        this.values.timeDistortion = 1 + (distortion / 100);
        updateAnimatorSpeed(this.values.timeDistortion);

        foreach (StatusEffect effect in this.values.buffs)
        {
            effect.updateTimeDistortion(distortion);
        }

        foreach (StatusEffect effect in this.values.debuffs)
        {
            effect.updateTimeDistortion(distortion);
        }
    }

    public void updateAnimatorSpeed(float value)
    {
        if (this.animator != null) this.animator.speed = value;
    }

    public void updateResource(CostType type, float addResource, bool showingDamageNumber)
    {
        //Mana Regeneration und Item Collect
        updateResource(type, null, addResource, showingDamageNumber);
    }

    public void updateResource(CostType type, float addResource)
    {
        //Life Regeneration und Player Init
        updateResource(type, null, addResource);
    }

    public void updateResource(CostType type, ItemGroup item, float addResource)
    {
        //Skill Target, Statuseffect und Price Reduce
        updateResource(type, item, addResource, true);
    }

    public virtual void reduceResource(Costs price)
    {
        //No Costs for AI
    }

    public virtual void updateResource(CostType type, ItemGroup item, float value, bool showingDamageNumber)
    {
        UpdateLifeMana(type, item, value, showingDamageNumber);
        CheckDeath();
    }

    public void UpdateLifeMana(CostType type, ItemGroup item, float value, bool showingDamageNumber)
    {
        switch (type)
        {
            case CostType.life:
                {
                    this.values.life = GameUtil.setResource(this.values.life, this.values.maxLife, value);

                    Color[] colorArray = MasterManager.staticValues.red;
                    if (value > 0) colorArray = MasterManager.staticValues.green;

                    if (this.values.life > 0
                        && this.values.currentState != CharacterState.dead
                        && showingDamageNumber) showDamageNumber(value, colorArray);

                    break;
                }
            case CostType.mana:
                {
                    this.values.mana = GameUtil.setResource(this.values.mana, this.values.maxMana, value);
                    if (showingDamageNumber && value > 0) showDamageNumber(value, MasterManager.staticValues.blue);
                    break;
                }
        }
    }

    #endregion

    #region Damage Functions

    private void showDamageNumber(float value, Color[] color)
    {
        if (this.stats.showDamageNumbers)
        {
            DamageNumbers damageNumberClone = Instantiate(MasterManager.damageNumber, this.transform.position, Quaternion.identity, this.transform);
            damageNumberClone.Initialize(value, color);
        }
    }

    public void gotHit(Skill skill, float percentage, bool knockback)
    {
        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

        if (this.values.currentState != CharacterState.dead
            && targetModule != null
            && ((!this.values.cantBeHit && !this.values.isInvincible) || targetModule.ignoreInvincibility))
        {
            //Status Effekt hinzufügen
            if (targetModule.statusEffects != null)
            {
                foreach (StatusEffect effect in targetModule.statusEffects)
                {
                    StatusEffectUtil.AddStatusEffect(effect, this);
                }
            }

            foreach (CharacterResource elem in targetModule.affectedResources)
            {
                float amount = elem.amount * percentage / 100;

                updateResource(elem.resourceType, elem.item, amount);

                if (this.values.life > 0 && elem.resourceType == CostType.life && amount < 0)
                {
                    if (this.GetComponent<AI>() != null
                     && this.GetComponent<AI>().aggroGameObject != null)
                        this.GetComponent<AI>().aggroGameObject.increaseAggroOnHit(skill.sender, elem.amount);

                    //Charakter-Treffer (Schaden) animieren
                    AudioUtil.playSoundEffect(this.gameObject, this.stats.hitSoundEffect);
                    SetInvincible();
                }
            }

            if (this.values.life > 0 && knockback)
            {
                //Rückstoß ermitteln
                float knockbackTrust = targetModule.thrust - (this.stats.antiKnockback / 100 * targetModule.thrust);
                knockBack(targetModule.knockbackTime, knockbackTrust, skill);
            }
        }
    }

    public void gotHit(Skill skill)
    {
        gotHit(skill, 100);
    }

    public void gotHit(Skill skill, float percentage)
    {
        gotHit(skill, percentage, true);
    }

    public virtual void Dead()
    {
        Dead(true);
    }

    public void Dead(bool showAnimation)
    {
        for (int i = 0; i < this.values.activeSkills.Count; i++)
        {
            resetCast(this.values.activeSkills[i]);
            if (this.values.activeSkills[i].isAttachedToSender()) this.values.activeSkills[i].DeactivateIt();
        }

        RemoveAllStatusEffects();
        this.values.currentState = CharacterState.dead;

        if (this.myRigidbody != null && this.myRigidbody.bodyType != RigidbodyType2D.Static) this.myRigidbody.velocity = Vector2.zero;
        if (this.boxCollider != null) this.boxCollider.enabled = false;
        if (this.shadowRenderer != null) this.shadowRenderer.enabled = false;

        //Play Death Effect
        if (showAnimation)
        {
            if (this.stats.deathAnimation != null) PlayDeathAnimation();
            else AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);
        }
        else DestroyItWithoutDrop();
    }

    public void DestroyIt()
    {
        dropItem();
        DestroyItWithoutDrop();
    }

    private void DestroyItWithoutDrop()
    {
        if (this.stats.hasRespawn) this.gameObject.SetActive(false);
        else Destroy(this.gameObject);
    }

    #endregion

    #region Knockback and Invincibility   

    public void SetInvincible() => SetInvincible(this.stats.cannotBeHitTime, true);

    public void SetInvincible(float delay, bool showHitcolor)
    {
        StopCoroutine(hitCo(delay, showHitcolor));
        StartCoroutine(hitCo(delay, showHitcolor));
    }

    public void SetInvincible(bool value)
    {
        this.values.cantBeHit = value;
    }

    public void setCannotDie(bool value) => this.cannotDie = value;

    public void KnockBack(float knockTime, float thrust, Vector2 direction)
    {
        if (this.myRigidbody != null
            && this.myRigidbody.bodyType != RigidbodyType2D.Kinematic)
        {
            Vector2 difference = direction.normalized * thrust;
            //this.myRigidbody.velocity = Vector2.zero;
            //this.myRigidbody.AddForce(difference, ForceMode2D.Impulse);
            this.myRigidbody.DOMove(this.myRigidbody.position + difference, knockTime);
            StartCoroutine(knockCo(knockTime));
        }
    }

    public void knockBack(float knockTime, float thrust, Skill attack)
    {
        if (this.myRigidbody != null)
        {
            Vector2 direction = this.myRigidbody.transform.position - attack.transform.position;
            KnockBack(knockTime, thrust, direction);
        }
    }

    #endregion

    #region Coroutines (Hit, Kill, Respawn, Knockback)

    private IEnumerator hitCo(float duration, bool showColor)
    {
        this.values.cantBeHit = true;
        if (this.stats.showHitcolor && showColor) this.ChangeColor(this.stats.hitColor);
        yield return new WaitForSeconds(duration);
        if (showColor) this.removeColor(this.stats.hitColor);
        this.values.cantBeHit = false;
    }

    private IEnumerator knockCo(float knockTime)
    {
        this.values.currentState = CharacterState.knockedback;
        yield return new WaitForSeconds(knockTime);

        //Rückstoß zurück setzten
        this.values.currentState = CharacterState.idle;
        this.myRigidbody.velocity = Vector2.zero;
    }

    #endregion

    #region Play

    public void PlaySoundEffect(AudioClip clip) => AudioUtil.playSoundEffect(this.gameObject, clip);

    public void PlayDeathAnimation()
    {
        if (this.activeDeathAnimation == null)
        {
            DeathAnimation deathObject = Instantiate(this.stats.deathAnimation, this.transform.position, Quaternion.identity);
            deathObject.setCharacter(this);
            this.activeDeathAnimation = deathObject;
        }
    }

    #endregion

    #region Get and Set

    public Vector2 GetShootingPosition()
    {
        if (this.skillStartPosition != null) return this.skillStartPosition.transform.position;
        return this.transform.position;
    }

    public virtual string GetCharacterName()
    {
        return this.stats.GetCharacterName();
    }

    public Vector2 GetGroundPosition()
    {
        if (this.shadowRenderer != null) return this.shadowRenderer.transform.position;
        return this.transform.position;
    }

    public Vector2 GetSpawnPosition()
    {
        return this.spawnPosition;
    }

    public float GetSpeedFactor()
    {
        return this.values.speedFactor;
    }

    #endregion

    #region misc

    public virtual bool HasEnoughCurrency(Costs price)
    {
        return true; //Override by Player and used by Ability
    }

    public bool canUseIt(Costs price)
    {
        //Door, Shop, Treasure, Abilities
        if (this.values.CanMove() && HasEnoughCurrency(price)) return true;
        return false;
    }

    public virtual void EnableScripts(bool value)
    {
        if (this.GetComponent<AIAggroSystem>() != null) this.GetComponent<AIAggroSystem>().enabled = value;
        if (this.GetComponent<AIEvents>() != null) this.GetComponent<AIEvents>().enabled = value;
        if (this.GetComponent<AIMovement>() != null) this.GetComponent<AIMovement>().enabled = value;

        this.boxCollider.enabled = value;
    }

    public void startAttackAnimation(string parameter)
    {
        AnimatorUtil.SetAnimatorParameter(this.animator, parameter);
    }

    public void resetCast(Skill skill)
    {
        if (skill != null) hideCastBarAndIndicator(skill);
    }

    public void hideCastBarAndIndicator(Skill skill)
    {
        if (this.activeCastbar != null) this.activeCastbar.destroyIt();
        if (skill.GetComponent<SkillAnimationModule>() != null) skill.GetComponent<SkillAnimationModule>().hideCastingAnimation();
    }

    #endregion

    #region Respawn

    public void SetCharacterSprites(bool value)
    {
        if (this.GetComponent<CharacterRenderingHandler>() != null)
            this.GetComponent<CharacterRenderingHandler>().enableSpriteRenderer(value);

        if (this.shadowRenderer != null) this.shadowRenderer.enabled = value;
    }

    public virtual void SpawnOut()
    {
        this.myRigidbody.velocity = Vector2.zero;
        this.EnableScripts(false);
        this.values.currentState = CharacterState.respawning;
    }

    public void SpawnIn()
    {
        this.values.currentState = CharacterState.idle;
        this.EnableScripts(true);
    }

    public void PlayDespawnAnimation()
    {
        AnimatorUtil.SetAnimatorParameter(this.animator, "Despawn");
    }

    public float GetDespawnLength()
    {
        return AnimatorUtil.GetAnimationLength(this.animator, "Respawn");
    }

    public void PlayRespawnAnimation()
    {
        AnimatorUtil.SetAnimatorParameter(this.animator, "Respawn");
    }

    #endregion

    #region StatusEffect

    private void UpdateStatusEffectGroup(List<StatusEffect> effects)
    {
        effects.RemoveAll(item => item == null);
        foreach (StatusEffect effect in effects) effect.Updating(this);
    }

    private void AddStatusEffectVisuals(List<StatusEffect> effects)
    {
        effects.RemoveAll(item => item == null);
        foreach (StatusEffect effect in effects) AddStatusEffectVisuals(effect);
    }

    private void AddStatusEffectVisuals(StatusEffect effect)
    {
        if (effect == null || effect.GetTarget() != this) return;
        if (effect.CanChangeColor()) ChangeColor(effect.GetColor());
        if (!ContainsEffect(effect)) this.statusEffectVisuals.Add(effect.Instantiate(this.activeStatusEffectParent));
    }

    private bool ContainsEffect(StatusEffect effect)
    {
        for (int i = 0; i < this.statusEffectVisuals.Count; i++)
        {
            if (this.statusEffectVisuals[i] != null && this.statusEffectVisuals[i].name == effect.GetVisuals().name) return true;
        }

        return false;
    }

    public void AddStatusEffectVisuals()
    {
        AddStatusEffectVisuals(this.values.buffs);
        AddStatusEffectVisuals(this.values.debuffs);
    }

    #endregion

    public void ShowDialog(string text, float duration)
    {        
        MiniDialogBox dialogBox = Instantiate(MasterManager.miniDialogBox, this.transform);
        float height = 0;
        if (this.headPosition != null) height = this.headPosition.transform.localPosition.y;
        dialogBox.setDialogBox(text, duration, height);        
    }

    public void RemoveAllStatusEffects()
    {
        StatusEffectUtil.RemoveAllStatusEffects(this.values.debuffs);
        StatusEffectUtil.RemoveAllStatusEffects(this.values.buffs);
    }
}
