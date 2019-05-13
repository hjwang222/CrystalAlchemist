using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#region Enums
public enum CharacterState
{
    walk,
    attack,
    interact, //in Reichweite eines interagierbaren Objektes
    inDialog, //Dialog-Box ist offen
    knockedback, //im Knockback
    idle,
    frozen, //kann sich nicht bewegen und angreifen
    silent, //kann nicht angreifen
    dead, 
    respawning
}

public enum CharacterType
{
    Player,
    Enemy,
    NPC,
    Object
}

public enum Gender
{
    male,
    female,
    none
}

#endregion



public class Character : MonoBehaviour
{
    #region Basic Attributes
    [Required("Name muss gesetzt sein!")]
    [BoxGroup("Pflichtfelder")]
    [Tooltip("Name")]
    public string characterName;

    ////////////////////////////////////////////////////////////////

    [TabGroup("Start-Values")]
    [Tooltip("Leben, mit dem der Spieler startet")]
    [Range(Utilities.minFloat, Utilities.maxFloatInfinite)]
    public float startLife = Utilities.minFloat;

    [TabGroup("Start-Values")]
    [Tooltip("Mana, mit dem der Spieler startet")]
    [Range(Utilities.minFloat, Utilities.maxFloatInfinite)]
    public float startMana = Utilities.minFloat;

    [TabGroup("Start-Values")]
    [Tooltip("Movement-Speed in %, mit dem der Spieler startet")]
    [Range(Utilities.minFloatPercent, Utilities.maxFloatPercent)]
    public float startSpeed = 100;

    [TabGroup("Start-Values")]
    [Tooltip("Geschwindigkeitsmodifier in % von Cooldown und Castzeit")]
    [Range(Utilities.minFloatPercent, Utilities.maxFloatPercent)]
    public float startSpellSpeed = 100;

    [Space(10)]
    [TabGroup("Start-Values")]
    [Tooltip("Respawn-Zeit")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float respawnTime = 30;

    [Space(10)]
    [TabGroup("Start-Values")]
    [Tooltip("Immunität von Statuseffekten")]
    public List<StatusEffect> immunityToStatusEffects = new List<StatusEffect>();



    [TabGroup("Max-Values")]
    [Tooltip("Maximales Life")]
    [Range(Utilities.minFloat, Utilities.maxFloatInfinite)]
    public float maxLife = Utilities.minFloat;

    [TabGroup("Max-Values")]
    [Tooltip("Maximales Mana")]
    [Range(Utilities.minFloat, Utilities.maxFloatInfinite)]
    public float maxMana = Utilities.minFloat;    



    [TabGroup("Regeneration")]
    [Tooltip("Höhe der Lebensregeneration")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float lifeRegeneration = 0;

    [TabGroup("Regeneration")]
    [Tooltip("Intervall der Lebensregeneration")]
    [Range(0, Utilities.maxFloatSmall)]
    public float lifeRegenerationInterval = 0;

    [Space(10)]
    [TabGroup("Regeneration")]
    [Tooltip("Höhe der Manaregeneration")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float manaRegeneration = 0;

    [TabGroup("Regeneration")]
    [Tooltip("Intervall der Manaregeneration")]
    [Range(0, Utilities.maxFloatSmall)]
    public float manaRegenerationInterval = 0;



    ////////////////////////////////////////////////////////////////

        
    [FoldoutGroup("Skills", expanded: false)]
    [Tooltip("Skills, welcher der Character verwenden kann")]
    public List<StandardSkill> skills = new List<StandardSkill>();

    [Space(10)]
    [FoldoutGroup("Skills", expanded: false)]
    [Tooltip("Skill, welcher der Character sofort verwendet")]
    public StandardSkill initializeSkill;

    [FoldoutGroup("Skills", expanded: false)]
    [Tooltip("Skill, welcher der Character bei seinem Tod verwendet")]
    public StandardSkill deathSkill;

    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Schaden", expanded: false)]
    [Required]
    [Tooltip("DamageNumber-Objekt hier rein (nur für zerstörbare Objekte)")]
    public GameObject damageNumber;

    [Space(10)]
    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Wie stark (-) oder schwach (+) kann das Objekt zurück gestoßen werden?")]
    [Range(-Utilities.maxFloatSmall, Utilities.maxFloatSmall)]
    public float antiKnockback = 0;

    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Unverwundbarkeitszeit")]
    [Range(0, 10)]
    public float cannotBeHitTime = 0.3f;

    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Kann das Objekt berührt werden?")]
    public bool isTouchable = true;

    [Space(10)]
    [FoldoutGroup("Schaden", expanded: false)]
    [Tooltip("Farbe, wenn Gegner getroffen wurde")]
    public bool showHitcolor = true;

    [FoldoutGroup("Schaden", expanded: false)]
    [ShowIf("showHitcolor")]
    public Color hitColor = Color.white;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Items und deren Wahrscheinlichkeit zwischen 1 und 100")]
    public LootTable[] lootTable;

    [Space(10)]
    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Multiloot = alle Items. Ansonsten nur das seltenste Item")]
    public bool multiLoot = false;

    [Space(10)]
    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Was darf der Charakter einsammeln. All = alles, ansonsten nur anhand der Liste")]
    public bool canCollectAll = false;

    [FoldoutGroup("Loot", expanded: false)]
    [HideIf("canCollectAll")]
    public List<ItemGroup> canCollect = new List<ItemGroup>();


    ////////////////////////////////////////////////////////////////
    

    [FoldoutGroup("Sound", expanded: false)]
    [Tooltip("Soundeffekt, wenn Gegner getroffen wurde")]
    public AudioClip hitSoundEffect;

    [FoldoutGroup("Sound", expanded: false)]
    [Tooltip("Soundeffekt, wenn Gegner getötet wurde")]
    public AudioClip killSoundEffect;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("RPG Elements", expanded: false)]
    [Tooltip("Rasse")]
    public string characterSpecies;

    [FoldoutGroup("RPG Elements", expanded: false)]
    [Tooltip("Geschlecht")]
    [EnumToggleButtons]
    public Gender characterGender = Gender.none;

    [FoldoutGroup("RPG Elements", expanded: false)]
    [Tooltip("Um welchen Typ handelt es sich?")]
    [EnumToggleButtons]
    public CharacterType characterType = CharacterType.Object;

    #endregion


    #region Attributes

    private float lifeTime;
    private float manaTime;
    private float speedMultiply = 5;
    private SimpleSignal healthSignal;
    private SimpleSignal manaSignal;
    private SimpleSignal keySignal;
    private SimpleSignal coinSignal;
    private SimpleSignal crystalSignal;
    private SimpleSignal woodSignal;
    private SimpleSignal stoneSignal;
    private SimpleSignal metalSignal;
    private Vector3 spawnPosition;

    [HideInInspector]
    public Rigidbody2D myRigidbody;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public CastBar castbar;
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
    public bool isInvincible;
    [HideInInspector]
    public bool isHit;
    [HideInInspector]
    public List<Item> items = new List<Item>();
    [HideInInspector]
    public List<StatusEffect> buffs = new List<StatusEffect>();
    [HideInInspector]
    public List<StatusEffect> debuffs = new List<StatusEffect>();
    [HideInInspector]
    public List<StandardSkill> activeSkills = new List<StandardSkill>();
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool lockAnimation = false;
    [HideInInspector]
    public float timeDistortion = 1;
    [HideInInspector]
    public GameObject activeLockOnTarget = null;
    [HideInInspector]
    public List<Item> inventory = new List<Item>();


    #endregion


    #region Start Functions (Spawn, Init)
    void Start()
    {
        init();
    }

    public void init()
    {
        this.spawnPosition = this.transform.position;
        this.direction = new Vector2(0, -1);
        //getItems();    
        
        setComponents();
        spawn();
        
        if (this.initializeSkill != null) useSkillInstantly(this.initializeSkill);
        //this.gameObject.layer = LayerMask.NameToLayer(this.gameObject.tag);
    }

    private void setComponents()
    {
        this.myRigidbody = GetComponent<Rigidbody2D>();
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.spriteRenderer != null) this.spriteRenderer.color = GlobalValues.color;
        this.animator = GetComponent<Animator>();
        this.transform.gameObject.tag = this.characterType.ToString();
    }


    public void spawn()
    {        
        if(this.currentState == CharacterState.respawning) Utilities.SetParameter(this.animator, "isRespawn", true);

        this.life = this.startLife;
        this.mana = this.startMana;

        //TODO
        this.speed = (this.startSpeed / 100) * this.speedMultiply;
        this.animator.speed = 1;

        this.updateTimeDistortion(0);
        //this.updateSpeed(0);
        this.updateSpellSpeed(0);

        this.buffs.Clear();
        this.debuffs.Clear();
        
        this.currentState = CharacterState.idle;
        this.animator.enabled = true;
        this.spriteRenderer.enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.transform.position = this.spawnPosition;

        Utilities.setItem(this.lootTable, this.multiLoot, this.items);
    }
    #endregion



    private void Update()
    {
        regeneration();        
    }

    public void setResourceSignal(SimpleSignal health, SimpleSignal mana, 
                                  SimpleSignal key, SimpleSignal coin, SimpleSignal crystal, 
                                  SimpleSignal wood, SimpleSignal stone, SimpleSignal metal)
    {
        this.healthSignal = health;
        this.manaSignal = mana;
        this.keySignal = key;
        this.coinSignal = coin;
        this.crystalSignal = crystal;
        this.woodSignal = wood;
        this.stoneSignal = stone;
        this.metalSignal = metal;
    }

    public void regeneration()
    {
        if (this.currentState != CharacterState.dead && this.currentState != CharacterState.respawning)
        {
            if (this.lifeRegeneration != 0 && this.lifeRegenerationInterval != 0)
            {
                if (this.lifeTime >= this.lifeRegenerationInterval)
                {
                    this.lifeTime = 0;
                    updateResource(ResourceType.life, null, this.lifeRegeneration);
                }
                else
                {
                    this.lifeTime += (Time.deltaTime * this.timeDistortion);
                }
            }
            if (this.manaRegeneration != 0 && this.manaRegenerationInterval != 0)
            {
                if (this.manaTime >= this.manaRegenerationInterval)
                {
                    this.manaTime = 0;
                    updateResource(ResourceType.mana, null, this.manaRegeneration);
                }
                else
                {
                    this.manaTime += (Time.deltaTime * this.timeDistortion);
                }
            }
       }
    }

    private void useSkillInstantly(StandardSkill skill)
    {        
        if (this.activeCastbar != null && skill.holdTimer == 0) this.activeCastbar.destroyIt();

        if (skill.cooldownTimeLeft > 0)
        {
            skill.cooldownTimeLeft -= (Time.deltaTime * this.timeDistortion);
        }
        else
        {
            int currentAmountOfSameSkills = getAmountOfSameSkills(skill);

            if (currentAmountOfSameSkills < skill.maxAmounts
                && this.getResource(skill.resourceType, skill.item) + skill.addResourceSender >= 0)
            {
                    if (!skill.isRapidFire && !skill.keepHoldTimer) skill.holdTimer = 0;
                    
                    skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown

                    if (skill.isStationary)
                    {
                    //Place it in World 
                        GameObject activeSkill = Instantiate(skill.gameObject, this.transform.position, Quaternion.identity);
                        activeSkill.GetComponent<StandardSkill>().sender = this;
                        this.activeSkills.Add(activeSkill.GetComponent<StandardSkill>());
                    }
                    else
                    {
                    //Place it as Child
                        GameObject activeSkill = Instantiate(skill.gameObject, this.transform.position, Quaternion.identity, this.transform);
                        activeSkill.GetComponent<StandardSkill>().sender = this;
                        this.activeSkills.Add(activeSkill.GetComponent<StandardSkill>());
                    }                
            }
        }
    }




    #region Day/Night Circle  

    public void updateColor()
    {
        if (this.spriteRenderer != null) this.spriteRenderer.color = GlobalValues.color;
    }

    #endregion


    #region SkillUsage


    #region Utils

    public int getAmountOfSameSkills(StandardSkill skill)
    {
        int result = 0;

        for (int i = 0; i < this.activeSkills.Count; i++)
        {
            StandardSkill activeSkill = this.activeSkills[i];
            if (activeSkill.skillName == skill.skillName) result++;
        }

        return result;
    }
    #endregion



    #endregion



    #region Item Functions (drop Item, Lootregeln)

    public void dropItem()
    {
        foreach (Item itemObject in this.items)
        {
            GameObject itemClone = Instantiate(itemObject.gameObject, this.transform.position, Quaternion.identity);
        }
    }

    #endregion


    #region Update Functions (Signals?)  

    //Signal?

    private void showDamageNumber(float addLife)
    {
        if (this.damageNumber != null)
        {
            GameObject damageNumberClone = Instantiate(this.damageNumber, this.transform.position, Quaternion.identity, this.transform);
            damageNumberClone.GetComponent<DamageNumbers>().number = addLife;
            damageNumberClone.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    private void killIt()
    {
        Utilities.SetParameter(this.animator, "isDead", true);

        this.currentState = CharacterState.dead;

        if (this.myRigidbody != null) this.myRigidbody.velocity = Vector2.zero;
        this.GetComponent<BoxCollider2D>().enabled = false;        
    }

    public void PlayDeathSoundEffect()
    {
        Utilities.playSoundEffect(this.audioSource, this.killSoundEffect);
    }

    public void DestroyIt()
    {
        dropItem();
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void updateResource(ResourceType type, Item item, float addResource)
    {
        switch (type)
        {
            case ResourceType.life:
                {
                    this.life = Utilities.setResource(this.life, this.maxLife, addResource);  
                    if(this.life > 0 && this.currentState != CharacterState.dead) showDamageNumber(addResource);
                    if (this.life <= 0) killIt();
                    callSignal(this.healthSignal, addResource);
                    break;
                }
            case ResourceType.mana:
                {
                    this.mana = Utilities.setResource(this.mana, this.maxMana, addResource);
                    callSignal(this.manaSignal, addResource);
                    break;
                }
            case ResourceType.item:
                {
                    if (item != null)
                    {
                        Utilities.updateInventory(item, this.inventory);
                        callSignal(this.woodSignal, addResource);  //TODO Single Signal?
                    }
                    break;
                }           
            /*case ResourceType.crystal:
                {
                    this.crystals = Mathf.RoundToInt(Utilities.setResource(this.crystals, this.maxCrystals, addResource));
                    callSignal(this.crystalSignal, addResource);
                    break;
                }*/
        }        
    }

    private void callSignal(SimpleSignal signal, float addResource)
    {
        if (signal != null && addResource != 0) signal.Raise();
    }

    public float getResource(ResourceType type, Item item)
    {
        //TODO ItemGroup?

        switch (type)
        {
            case ResourceType.life: return this.life;                
            case ResourceType.mana: return this.mana;
            case ResourceType.item: return Utilities.getAmountFromInventory(item.itemGroup, this.inventory, false);
        }

        return 0;
    }

    public float getMaxResource(ResourceType type, Item item)
    {
        switch (type)
        {
            case ResourceType.life: return this.maxLife;
            case ResourceType.mana: return this.maxMana;
            case ResourceType.item: return Utilities.getAmountFromInventory(item.itemGroup, this.inventory, true);
        }

        return 0;
    }


    public void updateSpeed(float addSpeed)
    {
        this.speed = ((this.startSpeed / 100) + (addSpeed / 100)) * this.timeDistortion * this.speedMultiply;
        this.animator.speed = this.speed / (this.startSpeed * this.speedMultiply / 100);
    }

    public void updateSpellSpeed(float addSpellSpeed)
    {
        this.spellspeed = ((this.startSpellSpeed/100) + (addSpellSpeed/100)) * this.timeDistortion;
    }

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion/100);

       /* if (this.CompareTag("Player"))
        {
            this.GetComponent<Player>().music.GetComponent<AudioSource>().pitch = this.timeDistortion;
        }*/

        if (this.animator != null) this.animator.speed = this.timeDistortion;
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

    #endregion


    #region Item Collect

    public void collect(Item item, bool destroyIt)
    {
        if (this.canCollectAll || this.canCollect.Contains(item.itemGroup))
        {
            item.playSounds();

            this.updateResource(item.resourceType, item, item.amount);         
            
            if (destroyIt) item.DestroyIt();
        }
    }

    #endregion


    #region Damage Functions (hit, statuseffect, knockback)

    public void gotHit(StandardSkill skill)
    {
        if (this.currentState != CharacterState.inDialog)
        {
            if (!this.isInvincible || skill.ignoreInvincibility)
            {
                //Status Effekt hinzufügen
                if (skill.statusEffects != null)
                {
                    foreach (StatusEffect effect in skill.statusEffects)
                    {
                        this.AddStatusEffect(effect);
                    }
                }

                //TODO ADDTARGET ITEM
                updateResource(ResourceType.life, null, skill.addLifeTarget);
                
                if(this.life > 0)
                {
                    if (skill.addLifeTarget < 0)
                    {
                        //Charakter-Treffer (Schaden) animieren
                        Utilities.playSoundEffect(this.audioSource, this.hitSoundEffect);
                        StartCoroutine(hitCo());
                    }

                    //Rückstoß ermitteln
                    float knockbackTrust = skill.thrust - antiKnockback;
                    knockBack(skill.knockbackTime, knockbackTrust, skill.transform);
                }
            }
        }
    }

    public void RemoveStatusEffect(StatusEffect statusEffect, bool allTheSame)
    {
        List<StatusEffect> statusEffects = null;
        List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

        if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = this.debuffs;
        else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = this.buffs;

        //Store in temp List to avoid Enumeration Exception
        foreach(StatusEffect effect in statusEffects)
        {
            if(effect.statusEffectName == statusEffect.statusEffectName)
            {
                dispellStatusEffects.Add(effect);
                if (!allTheSame) break;
            }
        }

        foreach (StatusEffect effect in dispellStatusEffects)
        {
            effect.DestroyIt();
        }

        dispellStatusEffects.Clear();
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect != null && this.characterType != CharacterType.Object)
        {
            bool isImmune = false;

            for(int i = 0; i< this.immunityToStatusEffects.Count; i++)
            {
                StatusEffect immunityEffect = this.immunityToStatusEffects[i];
                if(statusEffect.statusEffectName == immunityEffect.statusEffectName)
                {
                    isImmune = true;
                    break;
                }
            }

            if (!isImmune)
            {
                List<StatusEffect> statusEffects = null;
                List<StatusEffect> result = new List<StatusEffect>();

                //add to list for better reference
                if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = this.debuffs;
                else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = this.buffs;

                for (int i = 0; i < statusEffects.Count; i++)
                {
                    if (statusEffects[i].statusEffectName == statusEffect.statusEffectName)
                    {
                        //Hole alle gleichnamigen Effekte aus der Liste
                        result.Add(statusEffects[i]);
                    }
                }

                //TODO, das geht noch besser
                if (result.Count < statusEffect.maxStacks)
                {
                    //Wenn der Effekte die maximale Anzahl Stacks nicht überschritten hat -> Hinzufügen
                    instantiateStatusEffect(statusEffect, statusEffects);
                }
                else
                {
                    if (statusEffect.canOverride && statusEffect.endType == StatusEffectEndType.time)
                    {
                        //Wenn der Effekt überschreiben kann, soll der Effekt mit der kürzesten Dauer entfernt werden
                        StatusEffect toDestroy = result[0];
                        toDestroy.DestroyIt();

                        instantiateStatusEffect(statusEffect, statusEffects);
                    }
                    else if (statusEffect.canDeactivateIt && statusEffect.endType == StatusEffectEndType.mana)
                    {
                        StatusEffect toDestroy = result[0];
                        toDestroy.DestroyIt();
                    }
                }
            }       
        }
    }

    private void instantiateStatusEffect(StatusEffect statusEffect, List<StatusEffect> statusEffects)
    {
        GameObject statusEffectClone = Instantiate(statusEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
        DontDestroyOnLoad(statusEffectClone);
        StatusEffect statusEffectScript = statusEffectClone.GetComponent<StatusEffect>();
        statusEffectScript.target = this;
        //statusEffectClone.hideFlags = HideFlags.HideInHierarchy;

        //add to list for better reference
        statusEffects.Add(statusEffectClone.GetComponent<StatusEffect>());
    }

    private void knockBack(float knockTime, float thrust, Transform attack)
    {
        if (this.myRigidbody != null)
        {
            Vector2 diffference = this.myRigidbody.transform.position - attack.position;
            diffference = diffference.normalized * thrust;
            this.myRigidbody.AddForce(diffference, ForceMode2D.Impulse);
            StartCoroutine(knockCo(knockTime));
        }
    }

    #endregion


    #region Coroutines (Hit, Kill, Respawn, Knockback)

    public IEnumerator hitCo()
    {
        this.isInvincible = true;
        if(this.showHitcolor) this.spriteRenderer.color = this.hitColor;

        yield return new WaitForSeconds(this.cannotBeHitTime);

        this.isInvincible = false;
        this.spriteRenderer.color = GlobalValues.color;
    }
    

    public IEnumerator knockCo(float knockTime)
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
