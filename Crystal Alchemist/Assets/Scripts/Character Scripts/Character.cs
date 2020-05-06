using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


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
    [Required]
    [SerializeField]
    [Tooltip("Position des Skills")]
    private GameObject skillStartPosition;

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
        this.spawnPosition = this.transform.position;

        SetComponents();
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
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null) this.GetComponent<SpriteRendererExtensionHandler>().Initialize();
    }

    public void ResetValues()
    {
        this.values.Clear(this.stats);

        this.SetDefaultDirection();
        this.setStartColor();
        this.animator.speed = 1;
        this.updateTimeDistortion(0);
        this.updateSpellSpeed(0);

        this.animator.enabled = true;
        this.SetCharacterSprites(true);
        this.activeDeathAnimation = null;

        if (this.stats.isMassive) this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
        else this.myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        if (this.GetComponent<SpriteRendererExtensionHandler>() != null) this.GetComponent<SpriteRendererExtensionHandler>().resetColors();
        if (this.boxCollider != null) this.boxCollider.enabled = true;
    }

    public virtual void OnDestroy()
    {
        GameEvents.current.OnEffectAdded -= AddStatusEffectVisuals;
    }

    #endregion

    #region Updates

    public virtual void Update()
    {
        if (this.values.currentState == CharacterState.dead) return;

        Regenerate();
        UpdateLifeAnimation();
        UpdateStatusEffects();

        CheckDeath();
    }

    private void CheckDeath()
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

    public void UpdateAnimator(Vector2 direction) => AnimatorUtil.SetAnimDirection(direction, this.animator);

    public void ChangeDirection(Vector2 direction)
    {
        this.values.direction = direction;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) UpdateAnimator(Vector2.right);
            else if (direction.x < 0) UpdateAnimator(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0) UpdateAnimator(Vector2.up);
            else if (direction.y < 0) UpdateAnimator(Vector2.down);
        }
    }
    #endregion

    #region Color Changes

    public void setStartColor()
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().setStartColor();
    }

    public void removeColor(Color color)
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().removeColor(color);
    }

    public void ChangeColor(Color color)
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().changeColor(color);
    }

    public void setFlip()
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().flipSprite(this.values.direction);
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
        if (affectAnimation) this.animator.speed = this.values.speed / (this.stats.startSpeed * this.values.speedFactor / 100);
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
        switch (type)
        {
            case CostType.life:
                {
                    this.values.life = GameUtil.setResource(this.values.life, this.values.maxLife, value);

                    Color[] colorArray = MasterManager.staticValues.red;
                    if (value > 0) colorArray = MasterManager.staticValues.green;

                    if (this.values.life > 0 && this.values.currentState != CharacterState.dead && showingDamageNumber) showDamageNumber(value, colorArray);

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

        if (this.values.currentState != CharacterState.respawning
         && this.values.currentState != CharacterState.dead
         && targetModule != null)
        {
            if ((!this.values.cantBeHit && !this.values.isInvincible) || targetModule.ignoreInvincibility)
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
                        setInvincible();
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
        for (int i = 0; i < this.values.activeSkills.Count; i++)
        {
            resetCast(this.values.activeSkills[i]);
            if (this.values.activeSkills[i].isAttachedToSender()) this.values.activeSkills[i].DeactivateIt();
        }

        //TODO: Kill sofort (Skill noch aktiv)
        StatusEffectUtil.RemoveAllStatusEffects(this.values.debuffs);
        StatusEffectUtil.RemoveAllStatusEffects(this.values.buffs);

        this.setStartColor();
        this.values.currentState = CharacterState.dead;

        if (this.myRigidbody != null && this.myRigidbody.bodyType != RigidbodyType2D.Static) this.myRigidbody.velocity = Vector2.zero;
        if (this.boxCollider != null) this.boxCollider.enabled = false;
        this.shadowRenderer.enabled = false;

        //Play Death Effect
        if (this.stats.deathAnimation != null)
        {
            PlayDeathAnimation();
        }
        else AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);
    }

    public void DestroyIt()
    {
        dropItem();
        this.gameObject.SetActive(false);
    }

    public void DestroyItCompletely()
    {
        Destroy(this.gameObject);
    }

    #endregion

    #region Knockback and Invincibility   

    public void setInvincible() => setInvincible(this.stats.cannotBeHitTime, true);

    public void setInvincible(float delay, bool showHitcolor)
    {
        StopCoroutine(hitCo(delay, showHitcolor));
        StartCoroutine(hitCo(delay, showHitcolor));
    }

    public void setCannotDie(bool value) => this.cannotDie = value;

    public void knockBack(float knockTime, float thrust, Vector2 direction)
    {
        this.myRigidbody.velocity = Vector2.zero;
        Vector2 diffference = direction.normalized * thrust;
        this.myRigidbody.AddForce(diffference, ForceMode2D.Impulse);

        StartCoroutine(knockCo(knockTime));
    }

    public void knockBack(float knockTime, float thrust, Skill attack)
    {
        if (this.myRigidbody != null)
        {
            Vector2 diffference = this.myRigidbody.transform.position - attack.transform.position;
            knockBack(knockTime, thrust, diffference);
        }
    }

    #endregion

    #region Coroutines (Hit, Kill, Respawn, Knockback)

    private IEnumerator colliderDisable()
    {
        yield return null;
        if (this.boxCollider != null) this.boxCollider.enabled = false;
    }

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
        if (this.myRigidbody != null)
        {
            this.values.currentState = CharacterState.knockedback;
            yield return new WaitForSeconds(knockTime);

            //Rückstoß zurück setzten
            this.values.currentState = CharacterState.idle;
            this.myRigidbody.velocity = Vector2.zero;
        }
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
        if (this.values.ActiveInField() && HasEnoughCurrency(price)) return true;
        return false;
    }

    public void EnableScripts(bool value)
    {
        if (this.GetComponent<AIAggroSystem>() != null) this.GetComponent<AIAggroSystem>().enabled = value;
        if (this.GetComponent<AIEvents>() != null) this.GetComponent<AIEvents>().enabled = value;
        if (this.GetComponent<AIMovement>() != null) this.GetComponent<AIMovement>().enabled = value;

        if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().enabled = value;
        if (this.GetComponent<PlayerControls>() != null) this.GetComponent<PlayerControls>().enabled = value;
        if (this.GetComponent<PlayerMovement>() != null) this.GetComponent<PlayerMovement>().enabled = value;
        if (this.GetComponent<PlayerItems>() != null) this.GetComponent<PlayerItems>().enabled = value;
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
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().enableSpriteRenderer(value);

        if (this.shadowRenderer != null) this.shadowRenderer.enabled = value;
    }

    public virtual void SpawnOut()
    {
        this.myRigidbody.velocity = Vector2.zero;        
        this.EnableScripts(false);
        this.values.currentState = CharacterState.respawning;       
    }

    public virtual void SpawnIn()
    {
        this.ResetValues(); //NPC only        
        this.removeColor(Color.white);
        this.values.currentState = CharacterState.idle;
        this.EnableScripts(true);        
    }

    public void PlayRespawnAnimation()
    {
        this.ChangeColor(Color.white);
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
        if (effect == null) return;
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
}
