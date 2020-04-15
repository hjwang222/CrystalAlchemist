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
    public SpriteRenderer shadowRenderer;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject activeSkillParent;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject activeStatusEffectParent;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject skillStartPosition;

    #endregion


    #region Attributes

    private float regenTimeElapsed;
    private float manaTime;

    private DeathAnimation activeDeathAnimation;
    private bool cannotDie = false;

    [HideInInspector]
    public float speedMultiply = 5;
    [HideInInspector]
    public Vector3 spawnPosition;
    [HideInInspector]
    public CastBar activeCastbar;
    [HideInInspector]
    public CharacterState currentState;
    [HideInInspector]
    public float life;
    [HideInInspector]
    public float spellspeed;
    [HideInInspector]
    public float mana;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float maxLife;
    [HideInInspector]
    public float maxMana;
    [HideInInspector]
    public float lifeRegen;
    [HideInInspector]
    public float manaRegen;
    [HideInInspector]
    public int buffPlus;
    [HideInInspector]
    public int debuffMinus;
    [HideInInspector]
    public bool cantBeHit;
    [HideInInspector]
    public bool isInvincible = false;
    [HideInInspector]
    public bool isHit;

    [HideInInspector]
    public List<StatusEffect> buffs = new List<StatusEffect>();
    [HideInInspector]
    public List<StatusEffect> debuffs = new List<StatusEffect>();

    [HideInInspector]
    public List<Skill> activeSkills = new List<Skill>();
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool lockAnimation = false;
    [HideInInspector]
    public float timeDistortion = 1;
    [HideInInspector]
    public bool isPlayer = false;

    [HideInInspector]
    public List<Character> activePets = new List<Character>();
    [HideInInspector]
    public float steps = 0;
    [HideInInspector]
    public bool isOnIce = false;

    [HideInInspector]
    public ItemDrop itemDrop;

    #endregion


    #region Start Functions (Spawn, Init)
    public virtual void Awake()
    {
        init();
    }

    public void init()
    {
        if (!this.isPlayer) this.spawnPosition = this.transform.position;

        setComponents();
        setInternalAttributes();
        initSpawn(true);
    }

    private void setComponents()
    {
        if (this.myRigidbody == null) this.myRigidbody = this.GetComponent<Rigidbody2D>();
        if (this.skillStartPosition == null) this.skillStartPosition = this.gameObject;
        if (this.animator == null) this.animator = this.GetComponent<Animator>();
        if (this.boxCollider == null) this.boxCollider = GetComponent<Collider2D>();
        if (this.boxCollider != null) this.boxCollider.gameObject.tag = this.transform.gameObject.tag;
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null) this.GetComponent<SpriteRendererExtensionHandler>().init();
    }


    public void initSpawn(bool reset)
    {
        destroySkills();
        setBasicAttributesToNormal(reset);
        ActivateCharacter();
    }

    private void setInternalAttributes()
    {
        this.maxLife = this.stats.maxLife;
        this.maxMana = this.stats.maxMana;
        this.lifeRegen = this.stats.lifeRegeneration;
        this.manaRegen = this.stats.manaRegeneration;
        this.buffPlus = this.stats.buffPlus;
        this.debuffMinus = this.stats.debuffMinus;
    }

    private void setBasicAttributesToNormal(bool reset)
    {
        this.characterLookDown();

        //this.spriteRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
        this.setStartColor();

        if (reset)
        {
            this.life = this.stats.startLife;
            this.mana = this.stats.startMana;
            this.buffs.Clear();
            this.debuffs.Clear();

            //TODO
            this.speed = (this.stats.startSpeed / 100) * this.speedMultiply;
            this.animator.speed = 1;

            this.updateTimeDistortion(0);
            //this.updateSpeed(0);
            this.updateSpellSpeed(0);
        }

        this.currentState = CharacterState.idle;
        this.animator.enabled = true;
        this.enableSpriteRenderer(true);
        if (!this.isPlayer) this.transform.position = this.spawnPosition;

        this.activeDeathAnimation = null;

        if (this.stats.isMassive) this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
        else this.myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        if (this.GetComponent<SpriteRendererExtensionHandler>() != null) this.GetComponent<SpriteRendererExtensionHandler>().resetColors();
    }

    public void ActivateCharacter()
    {
        if (this.boxCollider != null) this.boxCollider.enabled = true;
        if (this.stats.lootTable != null) this.itemDrop = this.stats.lootTable.GetItemDrop();
    }
    #endregion

    public void spawnASAP()
    {
        this.gameObject.SetActive(true);
        this.PlayRespawnAnimation();
        this.initSpawn(true);
    }

    public virtual void prepareSpawnOut()
    {
        this.myRigidbody.velocity = Vector2.zero;
        this.currentState = CharacterState.respawning;
        this.enableSpriteRenderer(false);
        this.enableScripts(false); //wait until full Respawn
    }


    public void prepareSpawnFromAnimation(bool reset)
    {
        this.gameObject.SetActive(true);
        this.enableSpriteRenderer(true);
        this.enableScripts(false); //wait until full Respawn
        this.initSpawn(reset);
        this.PlayRespawnAnimation();
    }

    public void completeSpawnFromAnimation()
    {
        this.currentState = CharacterState.idle;
        this.enableScripts(true);
        this.enableSpriteRenderer(true);
        this.removeColor(Color.white);
    }

    #region Updates

    public virtual void Update()
    {
        if (this.currentState == CharacterState.dead)
            return;

        regeneration();
        updateLifeAnimation();
        updateStatusEffects();

        if (this.life <= 0
            && !this.cannotDie //Item
            && !this.isInvincible //Event
            && !this.cantBeHit) //after Hit
            KillIt();
    }

    private void updateStatusEffects()
    {
        updateStatusEffectGroup(this.buffs);
        updateStatusEffectGroup(this.debuffs);
    }

    private void updateStatusEffectGroup(List<StatusEffect> effects)
    {
        effects.RemoveAll(item => item == null);

        foreach (StatusEffect effect in effects)
        {
            effect.Updating();
        }
    }

    private void updateLifeAnimation()
    {
        float percentage = this.life * 100 / this.maxLife;
        AnimatorUtil.SetAnimatorParameter(this.animator, "Life", percentage);
    }

    private void regeneration()
    {
        if (this.currentState != CharacterState.dead && this.currentState != CharacterState.respawning)
        {
            if (this.stats.regenerationInterval != 0)
            {
                if (this.regenTimeElapsed >= this.stats.regenerationInterval)
                {
                    this.regenTimeElapsed = 0;
                    if (this.lifeRegen != 0 && this.life < this.maxLife) updateResource(CostType.life, this.lifeRegen);
                    if (this.manaRegen != 0 && this.mana < this.maxMana) updateResource(CostType.mana, this.manaRegen, false);
                }
                else
                {
                    this.regenTimeElapsed += (Time.deltaTime * this.timeDistortion);
                }
            }
        }
    }

    #endregion


    #region Item Functions (drop Item, Lootregeln)

    public void dropItem()
    {
        if (this.itemDrop != null) this.itemDrop.Instantiate(this.transform.position);
    }

    #endregion


    #region animation
    public void characterLookDown()
    {
        this.direction = new Vector2(0, -1);
        updateAnimDirection(this.direction);
    }

    public void updateAnimDirection(Vector2 direction)
    {
        //float move = Vector2.Dot(this.myRigidbody.velocity, this.direction);
        AnimatorUtil.SetAnimDirection(direction, this.animator);
    }

    public void changeAnim(Vector2 direction)
    {
        //TODO: To be tested
        this.direction = direction;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) updateAnimDirection(Vector2.right);
            else if (direction.x < 0) updateAnimDirection(Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0) updateAnimDirection(Vector2.up);
            else if (direction.y < 0) updateAnimDirection(Vector2.down);
        }
    }
    #endregion



    #region Update Functions (Signals?)  

    private void showDamageNumber(float value, Color[] color)
    {
        if (this.stats.showDamageNumbers)
        {
            DamageNumbers damageNumberClone = Instantiate(GlobalGameObjects.damageNumber, this.transform.position, Quaternion.identity, this.transform);
            damageNumberClone.Initialize(value, color);
        }
    }

    private void destroySkills()
    {
        //TODO: Exception
        foreach (Skill skill in this.activeSkills)
        {
            skill.DeactivateIt();
        }

        this.activeSkills.Clear();
    }

    public virtual void KillIt()
    {
        if (!this.isPlayer)
        {        
            for (int i = 0; i < this.activeSkills.Count; i++)
            {
                resetCast(this.activeSkills[i]);
                if (this.activeSkills[i].isAttachedToSender()) this.activeSkills[i].DeactivateIt();
            }

            //TODO: Kill sofort (Skill noch aktiv)
            StatusEffectUtil.RemoveAllStatusEffects(this.debuffs);
            StatusEffectUtil.RemoveAllStatusEffects(this.buffs);

            this.setStartColor();
            this.currentState = CharacterState.dead;

            if (this.myRigidbody != null && this.myRigidbody.bodyType != RigidbodyType2D.Static) this.myRigidbody.velocity = Vector2.zero;
            //StartCoroutine(colliderDisable());
            if (this.boxCollider != null) this.boxCollider.enabled = false;
            this.shadowRenderer.enabled = false;

            //if (this.GetComponent<AIEvents>() != null) this.GetComponent<AIEvents>().enabled = false;

            //Play Death Effect
            if (this.stats.deathAnimation != null)
            {
                PlayDeathAnimation();
            }
            else AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);
        }
    }


    public void PlaySoundEffect(AudioClip clip)
    {
        AudioUtil.playSoundEffect(this.gameObject, clip);
    }

    public void DestroyIt()
    {
        dropItem();
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void DestroyItCompletely()
    {
        Destroy(this.gameObject);
    }

    public void PlayDeathAnimation()
    {
        if (this.activeDeathAnimation == null)
        {
            DeathAnimation deathObject = Instantiate(this.stats.deathAnimation, this.transform.position, Quaternion.identity);
            deathObject.setCharacter(this);
            this.activeDeathAnimation = deathObject;
        }
    }

    public void PlayRespawnAnimation()
    {
        this.changeColor(Color.white);
        AnimatorUtil.SetAnimatorParameter(this.animator, "Respawn");
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
                    this.life = GameUtil.setResource(this.life, this.maxLife, value);

                    Color[] colorArray = GlobalGameObjects.staticValues.red;
                    if (value > 0) colorArray = GlobalGameObjects.staticValues.green;

                    if (this.life > 0 && this.currentState != CharacterState.dead && showingDamageNumber) showDamageNumber(value, colorArray);

                    break;
                }
            case CostType.mana:
                {
                    this.mana = GameUtil.setResource(this.mana, this.maxMana, value);
                    if (showingDamageNumber && value > 0) showDamageNumber(value, GlobalGameObjects.staticValues.blue);
                    break;
                }
        }
    }


    public void callSignal(SimpleSignal signal, float addResource)
    {
        if (signal != null && addResource != 0) signal.Raise();
    }

    public virtual bool HasEnoughCurrency(Costs price)
    {
        //Override by Player
        //Used by Ability
        return true;
    }

    public bool ActiveInField()
    {
        if (this.currentState != CharacterState.inDialog
            && this.currentState != CharacterState.respawning
            && this.currentState != CharacterState.inMenu
            && this.currentState != CharacterState.dead
            && !StatusEffectUtil.isCharacterStunned(this)) return true;
        return false;
    }

    public bool canUseIt(Costs price)
    {
        //Door, Shop, Treasure, Abilities
        if (ActiveInField() && HasEnoughCurrency(price)) return true;       
        return false;
    }

    public void updateSpeed(int addSpeed)
    {
        updateSpeed(addSpeed, true);
    }

    public void updateSpeed(int addSpeed, bool affectAnimation)
    {
        float startSpeedInPercent = this.stats.startSpeed / 100;
        float addNewSpeed = startSpeedInPercent * ((float)addSpeed / 100);
        float changeSpeed = startSpeedInPercent + addNewSpeed;

        this.speed = changeSpeed * this.timeDistortion * this.speedMultiply;
        if (affectAnimation) this.animator.speed = this.speed / (this.stats.startSpeed * this.speedMultiply / 100);
    }

    public void updateSpellSpeed(float addSpellSpeed)
    {
        this.spellspeed = ((this.stats.startSpellSpeed / 100) + (addSpellSpeed / 100)) * this.timeDistortion;
    }

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion / 100);

        updateAnimatorSpeed(this.timeDistortion);

        foreach (StatusEffect effect in this.buffs)
        {
            effect.updateTimeDistortion(distortion);
        }

        foreach (StatusEffect effect in this.debuffs)
        {
            effect.updateTimeDistortion(distortion);
        }
    }

    public void updateAnimatorSpeed(float value)
    {
        if (this.animator != null) this.animator.speed = value;
    }

    #endregion

    public void enableScripts(bool value)
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
        if (skill != null)
        {
            //if (!skill.keepHoldTimer) skill.holdTimer = 0;
            hideCastBarAndIndicator(skill);
        }
    }

    public void hideCastBarAndIndicator(Skill skill)
    {
        if (this.activeCastbar != null)
        {
            this.activeCastbar.destroyIt();
        }

        if (skill.GetComponent<SkillAnimationModule>() != null) skill.GetComponent<SkillAnimationModule>().hideCastingAnimation();
    }


    #region Color Changes

    public void enableSpriteRenderer(bool value)
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().enableSpriteRenderer(value);

        if (this.shadowRenderer != null) this.shadowRenderer.enabled = value;
    }

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

    public void changeColor(Color color)
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().changeColor(color);
    }

    public void setFlip()
    {
        if (this.GetComponent<SpriteRendererExtensionHandler>() != null)
            this.GetComponent<SpriteRendererExtensionHandler>().flipSprite(this.direction);
    }

    #endregion


    #region Damage Functions (hit, statuseffect, knockback)

    public void gotHit(Skill skill, float percentage, bool knockback)
    {
        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

        if (this.currentState != CharacterState.respawning
         && this.currentState != CharacterState.dead
         && targetModule != null)
        {
            if ((!this.cantBeHit && !this.isInvincible) || targetModule.ignoreInvincibility)
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

                    if (this.life > 0 && elem.resourceType == CostType.life && amount < 0)
                    {
                        if (this.GetComponent<AI>() != null
                         && this.GetComponent<AI>().aggroGameObject != null)
                            this.GetComponent<AI>().aggroGameObject.increaseAggroOnHit(skill.sender, elem.amount);

                        //Charakter-Treffer (Schaden) animieren
                        AudioUtil.playSoundEffect(this.gameObject, this.stats.hitSoundEffect);
                        setInvincible();
                    }
                }

                if (this.life > 0 && knockback)
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

    public void setInvincible()
    {
        setInvincible(this.stats.cannotBeHitTime, true);
    }

    public void setInvincible(float delay, bool showHitcolor)
    {
        StopCoroutine(hitCo(delay, showHitcolor));
        StartCoroutine(hitCo(delay, showHitcolor));
    }

    public void setCannotDie(bool value)
    {
        this.cannotDie = value;
    }

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
        this.cantBeHit = true;

        if (this.stats.showHitcolor && showColor) this.changeColor(this.stats.hitColor);

        yield return new WaitForSeconds(duration);

        if (showColor) this.removeColor(this.stats.hitColor);

        this.cantBeHit = false;
    }

    private IEnumerator knockCo(float knockTime)
    {
        if (this.myRigidbody != null)
        {
            this.currentState = CharacterState.knockedback;

            yield return new WaitForSeconds(knockTime);

            //Rückstoß zurück setzten
            this.currentState = CharacterState.idle;
            this.myRigidbody.velocity = Vector2.zero;
        }
    }

    #endregion



    private void StartCountDown()
    {
        //start a 10 second countdown
        StartCoroutine(CountDown(10));
    }

    private IEnumerator CountDown(float seconds)
    {
        //do something before timer starts

        yield return new WaitForSeconds(seconds);

        //do something when timer is over
    }



}
