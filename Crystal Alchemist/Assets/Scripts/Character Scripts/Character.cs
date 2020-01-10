﻿using System.Collections;
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
    public SpriteRenderer spriteRenderer;

    [Required]
    [BoxGroup("Easy Access")]
    public Collider2D boxCollider;

    [BoxGroup("Easy Access")]
    [Required]
    public SpriteRenderer shadowRenderer;

    [BoxGroup("Easy Access")]
    [Required]
    public Sprite startSpriteForRespawn;

    [BoxGroup("Easy Access")]
    [Required]
    public Sprite startSpriteForRespawnWhite;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject activeSkillParent;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject activeStatusEffectParent;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject skillSetParent;

    [BoxGroup("Easy Access")]
    [Required]
    public GameObject shootingPosition;

    #endregion

    #region Attributes

    private float regenTimeElapsed;
    private float manaTime;
    private List<Color> colors = new List<Color>();
    private bool showTargetHelp = false;
    private GameObject targetHelpObjectPlayer;
    private DeathAnimation activeDeathAnimation;
    private float immortalAtStart = 0;

    [HideInInspector]
    public float speedMultiply = 5;
    [HideInInspector]
    public Vector3 spawnPosition;
    [HideInInspector]
    public AudioSource audioSource;
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


    public float maxLife;
    public float maxMana;
    public float lifeRegen;
    public float manaRegen;
    public int buffPlus;
    public int debuffMinus;




    [HideInInspector]
    public bool isInvincible;
    [HideInInspector]
    public bool isImmortal = false;
    [HideInInspector]
    public bool cannotDie = false;
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
    public TargetingSystem activeLockOnTarget = null;
    [HideInInspector]
    public bool isPlayer = false;
    [HideInInspector]
    public List<Item> inventory = new List<Item>();
    [HideInInspector]
    public List<Character> activePets = new List<Character>();
    [HideInInspector]
    public float steps = 0;
    [HideInInspector]
    public bool isOnIce = false;


    #endregion


    #region Start Functions (Spawn, Init)
    private void Awake()
    {
        init();
    }

    public void init()
    {
        if (this.immortalAtStart > 0) this.setImmortal(this.immortalAtStart);
        if (!this.isPlayer) this.spawnPosition = this.transform.position;

        setComponents();
        setInternalAttributes();
        initSpawn(true);
    }

    private void setComponents()
    {
        if (this.myRigidbody == null) this.myRigidbody = this.GetComponent<Rigidbody2D>();
        if (this.spriteRenderer == null) this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (this.animator == null) this.animator = this.GetComponent<Animator>();
        if (this.boxCollider == null) this.boxCollider = GetComponent<Collider2D>();

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        this.colors.Add(this.spriteRenderer.color);

        this.transform.gameObject.tag = this.stats.characterType.ToString();
        if (this.boxCollider != null) this.boxCollider.gameObject.tag = this.transform.gameObject.tag;
    }

    public void setTargetHelper(GameObject targetHelper)
    {
        this.targetHelpObjectPlayer = targetHelper;
        this.targetHelpObjectPlayer.SetActive(false);
    }

    public void setTargetHelperActive(bool value)
    {
        if (this.targetHelpObjectPlayer != null) this.targetHelpObjectPlayer.gameObject.SetActive(value);
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
        this.direction = new Vector2(0, -1);

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
        this.spriteRenderer.enabled = true;

        this.shadowRenderer.enabled = true;
        if(!this.isPlayer) this.transform.position = this.spawnPosition;

        this.activeDeathAnimation = null;

        if (this.stats.isMassive) this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;

        resetColor();
    }

    public void ActivateCharacter()
    {
        if (this.boxCollider != null) this.boxCollider.enabled = true;
        Utilities.Items.setItem(this.stats.lootTable, this.stats.multiLoot, this.inventory);

        AIEvents eventAI = this.GetComponent<AIEvents>();
        if (eventAI != null) eventAI.init();
    }
    #endregion


    #region Updates

    public void Update()
    {
        if (this.currentState == CharacterState.dead)
            return;

        regeneration();
        updateLifeAnimation();

        if (this.life <= 0 && !this.cannotDie && !this.isImmortal && !this.isInvincible)
            KillIt();
    }

    private void updateLifeAnimation()
    {
        float percentage = this.life * 100 / this.maxLife;
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Life", percentage);
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
                    if (this.lifeRegen != 0 && this.life < this.maxLife) updateResource(ResourceType.life, null, this.lifeRegen);
                    if (this.manaRegen != 0 && this.mana < this.maxMana) updateResource(ResourceType.mana, null, this.manaRegen, false);
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
        foreach (Item itemObject in this.inventory)
        {
            GameObject itemClone = Instantiate(itemObject.gameObject, this.transform.position, Quaternion.identity);
        }
    }

    #endregion


    #region Update Functions (Signals?)  

    private void showDamageNumber(float value, Color[] color)
    {
        if (this.stats.damageNumber != null)
        {
            GameObject damageNumberClone = Instantiate(this.stats.damageNumber, this.transform.position, Quaternion.identity, this.transform);
            damageNumberClone.GetComponent<DamageNumbers>().number = value;
            damageNumberClone.GetComponent<DamageNumbers>().setcolor(color);
            damageNumberClone.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    private void destroySkills()
    {
        //TODO: Exception
        foreach (Skill skill in this.activeSkills)
        {
            skill.durationTimeLeft = 0;
        }

        this.activeSkills.Clear();
    }

    public virtual void KillIt()
    {
        if (!this.isPlayer)
        {
            //TODO:Destroy on Cast            

            for(int i = 0; i< this.activeSkills.Count; i++)
            {               
                if (this.activeSkills[i].attachToSender) this.activeSkills[i].DestroyIt();
            }
            
            //TODO: Kill sofort (Skill noch aktiv)
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.debuffs);
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.buffs);

            this.spriteRenderer.color = Color.white;
            this.currentState = CharacterState.dead;

            if (this.myRigidbody != null && this.myRigidbody.bodyType != RigidbodyType2D.Static) this.myRigidbody.velocity = Vector2.zero;
            //StartCoroutine(colliderDisable());
            if (this.boxCollider != null) this.boxCollider.enabled = false;
            this.shadowRenderer.enabled = false;

            //Play Death Effect
            if (this.stats.deathAnimation != null)
            {
                PlayDeathAnimation();
            }
            else Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Dead");
        }
    }


    public void PlaySoundEffect(AudioClip clip)
    {
        Utilities.Audio.playSoundEffect(this.audioSource, clip);
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

    public void updateResource(ResourceType type, Item item, float addResource)
    {
        updateResource(type, item, addResource, true);
    }

    public virtual void updateResource(ResourceType type, Item item, float value, bool showingDamageNumber)
    {
        switch (type)
        {
            case ResourceType.life:
                {
                    this.life = Utilities.Resources.setResource(this.life, this.maxLife, value);

                    Color[] colorArray = GlobalValues.red;
                    if (value > 0) colorArray = GlobalValues.green;

                    if (this.life > 0 && this.currentState != CharacterState.dead && showingDamageNumber) showDamageNumber(value, colorArray);
                    
                    break;
                }
            case ResourceType.mana:
                {
                    this.mana = Utilities.Resources.setResource(this.mana, this.maxMana, value);
                    if (showingDamageNumber && value > 0) showDamageNumber(value, GlobalValues.blue);
                    break;
                }
            case ResourceType.item:
                {
                    if (item != null)
                    {
                        Utilities.Items.updateInventory(item, this, Mathf.RoundToInt(value));
                        callSignal(item.signal, value);
                    }
                    break;
                }
            case ResourceType.skill:
                {
                    if (item != null && item.skill != null && this.GetComponent<Player>() != null)
                    {
                        Utilities.Skills.updateSkillset(item.skill, this.GetComponent<Player>());
                    }
                    break;
                }
            case ResourceType.statuseffect:
                {
                    if (item != null && item.statusEffects.Count > 0)
                    {
                        foreach (StatusEffect effect in item.statusEffects)
                        {
                            Utilities.StatusEffectUtil.AddStatusEffect(effect, this);
                        }
                    }
                    break;
                }
        }
    }


    public void callSignal(SimpleSignal signal, float addResource)
    {
        if (signal != null && addResource != 0) signal.Raise();
    }

    public float getResource(ResourceType type, Item item)
    {
        switch (type)
        {
            case ResourceType.life: return this.life;
            case ResourceType.mana: return this.mana;
            case ResourceType.item: return Utilities.Items.getAmountFromInventory(item, this.inventory, false);
        }

        return 0;
    }

    public float getMaxResource(ResourceType type, Item item)
    {
        switch (type)
        {
            case ResourceType.life: return this.maxLife;
            case ResourceType.mana: return this.maxMana;
            case ResourceType.item: return Utilities.Items.getAmountFromInventory(item, this.inventory, true);
        }

        return 0;
    }

    public void updateSpeed(float addSpeed)
    {
        updateSpeed(addSpeed, true);
    }

    public void updateSpeed(float addSpeed, bool affectAnimation)
    {
        float startSpeedInPercent = this.stats.startSpeed / 100;
        float addNewSpeed = startSpeedInPercent * (addSpeed / 100);
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

         /*if (this.CompareTag("Player"))
         {
             this.GetComponent<Player>().music.GetComponent<AudioSource>().pitch = this.timeDistortion;
         }*/

        updateAnimatorSpeed(this.timeDistortion);

        if (this.audioSource != null) this.audioSource.pitch = this.timeDistortion;

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


    public void startAttackAnimation(string parameter)
    {
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, parameter);
    }

    public void resetCast(Skill skill)
    {
        if (skill != null)
        {
            if (!skill.keepHoldTimer) skill.holdTimer = 0;
            hideCastBarAndIndicator(skill);
        }
    }

    public void hideCastBarAndIndicator(Skill skill)
    {
        if (this.activeCastbar != null)
        {
            this.activeCastbar.destroyIt();
        }

        if (skill.GetComponent<SkillIndicatorModule>() != null) skill.GetComponent<SkillIndicatorModule>().hideIndicator();
        if (skill.GetComponent<SkillAnimationModule>() != null) skill.GetComponent<SkillAnimationModule>().hideCastingAnimation();
    }


    #region Color Changes

    public void resetColor()
    {
        if (this.spriteRenderer != null)
        {
            if (this.colors.Count > 0) this.spriteRenderer.color = this.colors[0];
            this.colors.Clear();
            this.addColor(this.spriteRenderer.color);
        }
    }

    public void resetColor(Color color)
    {
        if (this.spriteRenderer != null)
        {
            this.colors.Remove(color);
            this.spriteRenderer.color = this.colors[this.colors.Count - 1];
        }
    }

    public void enableSpriteRenderer(bool value)
    {
        this.spriteRenderer.enabled = value;
        if(this.shadowRenderer != null) this.shadowRenderer.enabled = value;
    }

    public void addColor(Color color)
    {
        if (this.colors.Contains(color))
        {
            this.spriteRenderer.color = this.colors[this.colors.IndexOf(color)];
        }
        else
        {
            this.colors.Add(color);
            this.spriteRenderer.color = this.colors[this.colors.Count - 1];
        }
    }

    #endregion


    #region Item Collect

    public void collect(Item item, bool destroyIt)
    {
        collect(item, destroyIt, true);
    }

    public void collect(Item item, bool destroyIt, bool playSound)
    {
        if (this.stats.canCollectAll || this.stats.canCollect.Contains(item.itemGroup))
        {
            if (playSound) item.playSounds();

            this.updateResource(item.resourceType, item, item.getTotalAmount());

            if (destroyIt) item.DestroyIt();
        }
    }

    #endregion


    #region Damage Functions (hit, statuseffect, knockback)

    public void gotHit(Skill skill, float percentage)
    {
        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

        if (this.currentState != CharacterState.respawning
         && this.currentState != CharacterState.dead
         && targetModule != null)
        {
            if ((!this.isInvincible && !this.isImmortal) || targetModule.ignoreInvincibility)
            {
                //Status Effekt hinzufügen
                if (targetModule.statusEffects != null)
                {
                    foreach (StatusEffect effect in targetModule.statusEffects)
                    {
                        Utilities.StatusEffectUtil.AddStatusEffect(effect, this);
                    }
                }

                foreach (affectedResource elem in targetModule.affectedResources)
                {
                    float amount = elem.amount * percentage / 100;

                    updateResource(elem.resourceType, null, amount);

                    if (this.life > 0 && elem.resourceType == ResourceType.life && amount < 0)
                    {
                        if (this.GetComponent<AI>() != null
                         && this.GetComponent<AI>().aggroGameObject != null)
                            this.GetComponent<AI>().aggroGameObject.increaseAggroOnHit(skill.sender, elem.amount);

                        //Charakter-Treffer (Schaden) animieren
                        Utilities.Audio.playSoundEffect(this.audioSource, this.stats.hitSoundEffect);
                        StartCoroutine(hitCo());
                    }
                }

                if (this.life > 0)
                {
                    //Rückstoß ermitteln
                    float knockbackTrust = targetModule.thrust - (this.stats.antiKnockback/100* targetModule.thrust);
                    knockBack(targetModule.knockbackTime, knockbackTrust, skill);
                }
            }
        }
    }

    public void gotHit(Skill skill)
    {
        gotHit(skill, 100);
    }

    public void setImmortalAtStart(float duration)
    {
        this.immortalAtStart = duration;
    }

    public void setImmortal(float duration)
    {
        StartCoroutine(immortalCo(duration));
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

    private IEnumerator hitCo()
    {
        this.isInvincible = true;
        if (this.stats.showHitcolor) this.addColor(this.stats.hitColor);

        yield return new WaitForSeconds(this.stats.cannotBeHitTime);
        this.resetColor(this.stats.hitColor);
        this.isInvincible = false;
    }

    private IEnumerator immortalCo(float duration)
    {
        this.isImmortal = true;
        yield return new WaitForSeconds(duration);
        this.isImmortal = false;
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
}
