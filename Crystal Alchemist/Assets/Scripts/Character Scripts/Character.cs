using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

#region Enums
public enum CharacterState
{
    walk,
    attack,
    defend,
    interact, //in Reichweite eines interagierbaren Objektes
    inDialog, //Dialog-Box ist offen
    inMenu, //Pause oder Inventar ist offen
    knockedback, //im Knockback
    idle,
    silent, //kann nicht angreifen
    dead,
    manually,
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
    [Required]
    [BoxGroup("Pflichtfelder")]
    public string characterName;

    [BoxGroup("Pflichtfelder")]
    public string englischCharacterName;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public Rigidbody2D myRigidbody;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public Animator animator;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public SpriteRenderer spriteRenderer;

    [Required]
    [BoxGroup("Pflichtfelder")]
    public BoxCollider2D boxCollider;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public SpriteRenderer shadowRenderer;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public Sprite startSpriteForRespawn;

    [BoxGroup("Pflichtfelder")]
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
    [Tooltip("Immunität von Statuseffekten")]
    public List<StatusEffect> immunityToStatusEffects = new List<StatusEffect>();



    [TabGroup("Spawn Values")]
    [Tooltip("Maximales Life")]
    [Range(Utilities.minFloat, Utilities.maxFloatInfinite)]
    public float maxLife = Utilities.minFloat;

    [TabGroup("Spawn Values")]
    [Tooltip("Maximales Mana")]
    [Range(Utilities.minFloat, Utilities.maxFloatInfinite)]
    public float maxMana = Utilities.minFloat;

    [Space(10)]
    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Zeit")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float respawnTime = 30;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Animation")]
    public RespawnAnimation respawnAnimation;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Animation")]
    public DeathAnimation deathAnimation;


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
    public List<string> canCollect = new List<string>();


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
    public float speedMultiply = 5;
    private SimpleSignal healthSignal;
    private SimpleSignal manaSignal;
    private SimpleSignal currencies;
    private List<Color> colors = new List<Color>();
    private bool showTargetHelp = false;
    private GameObject targetHelpObjectPlayer;
    private DeathAnimation activeDeathAnimation;

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
    [HideInInspector]
    public bool isInvincible;
    [HideInInspector]
    public bool isImmortal = false;
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
        initSpawn();

        //this.gameObject.layer = LayerMask.NameToLayer(this.gameObject.tag);
    }

    private void setComponents()
    {
        if (this.myRigidbody == null) this.myRigidbody = this.GetComponent<Rigidbody2D>();
        if (this.spriteRenderer == null) this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (this.animator == null) this.animator = this.GetComponent<Animator>();
        if (this.boxCollider == null) this.boxCollider = GetComponent<BoxCollider2D>();

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        this.colors.Add(this.spriteRenderer.color);

        this.transform.gameObject.tag = this.characterType.ToString();
        /*
        if (this.spriteRenderer != null)
        {
            this.spriteRenderer.gameObject.tag = this.transform.gameObject.tag;
        }*/
        if (this.boxCollider != null) this.boxCollider.gameObject.tag = this.transform.gameObject.tag;
    }

    public void setResourceSignal(SimpleSignal health, SimpleSignal mana,
                                  SimpleSignal currencies)
    {
        this.healthSignal = health;
        this.manaSignal = mana;
        this.currencies = currencies;
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

    public void initSpawn()
    {
        setBasicAttributesToNormal();
        ActivateCharacter();
    }

    private void setBasicAttributesToNormal()
    {
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

        this.shadowRenderer.enabled = true;
        this.transform.position = this.spawnPosition;

        this.activeDeathAnimation = null;

        resetColor();
    }

    public void ActivateCharacter()
    {
        if (this.boxCollider != null) this.boxCollider.enabled = true;
        Utilities.Items.setItem(this.lootTable, this.multiLoot, this.items);

        AIEvents eventAI = this.GetComponent<AIEvents>();
        if (eventAI != null) eventAI.init();
    }
    #endregion


    #region Updates

    public void Update()
    {
        regeneration();

        if (this.currentState != CharacterState.knockedback && !this.isOnIce)
        {
            this.myRigidbody.velocity = Vector2.zero;
        }

        if (this.currentState == CharacterState.dead)
            return;
    }

    private void regeneration()
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
                    updateResource(ResourceType.mana, null, this.manaRegeneration, false);
                }
                else
                {
                    this.manaTime += (Time.deltaTime * this.timeDistortion);
                }
            }
        }
    }

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

    private void showDamageNumber(float value, Color[] color)
    {
        if (this.damageNumber != null)
        {
            GameObject damageNumberClone = Instantiate(this.damageNumber, this.transform.position, Quaternion.identity, this.transform);
            damageNumberClone.GetComponent<DamageNumbers>().number = value;
            damageNumberClone.GetComponent<DamageNumbers>().setcolor(color);
            damageNumberClone.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    private void destroySkills()
    {
        //TODO: Exception
        foreach (StandardSkill skill in this.activeSkills)
        {
            skill.durationTimeLeft = 0;
        }

        this.activeSkills.Clear();
    }

    public void KillIt()
    {
        if (this.isPlayer)
        {
            //TODO: Wenn Spieler tot ist
            SceneManager.LoadSceneAsync(0);
        }
        else
        {
            //TODO: Kill sofort (Skill noch aktiv)
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.debuffs);
            Utilities.StatusEffectUtil.RemoveAllStatusEffects(this.buffs);
                       
            this.spriteRenderer.color = Color.white;

            AIAggroSystem aggro = this.GetComponent<AIAggroSystem>();
            if (aggro != null) aggro.clearAggro();

            this.currentState = CharacterState.dead;

            if (this.myRigidbody != null) this.myRigidbody.velocity = Vector2.zero;
            //StartCoroutine(colliderDisable());
            if (this.boxCollider != null) this.boxCollider.enabled = false;
            this.shadowRenderer.enabled = false;

            destroySkills();

            //Play Death Effect
            if (this.deathAnimation != null)
            {
                PlayDeathAnimation();
            }
            else Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Dead");
        }
    }


    public void PlayDeathSoundEffect()
    {
        Utilities.Audio.playSoundEffect(this.audioSource, this.killSoundEffect);
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
            DeathAnimation deathObject = Instantiate(this.deathAnimation, this.transform.position, Quaternion.identity);
            deathObject.setCharacter(this);
            this.activeDeathAnimation = deathObject;
        }
    }

    public void updateResource(ResourceType type, Item item, float addResource)
    {
        updateResource(false, type, item, addResource, true);
    }

    public void updateResource(ResourceType type, Item item, float addResource, bool showingDamageNumber)
    {
        updateResource(false, type, item, addResource, showingDamageNumber);
    }

    public void updateResource(bool raiseResourceSignal, ResourceType type, Item item, float addResource)
    {
        updateResource(raiseResourceSignal, type, item, addResource, true);
    }

    public void updateResource(bool raiseResourceSignal, ResourceType type, Item item, float value, bool showingDamageNumber)
    {
        switch (type)
        {
            case ResourceType.life:
                {
                    this.life = Utilities.Resources.setResource(this.life, this.maxLife, value);

                    Color[] colorArray = GlobalValues.red;
                    if (value > 0) colorArray = GlobalValues.green;

                    if (this.life > 0 && this.currentState != CharacterState.dead && showingDamageNumber) showDamageNumber(value, colorArray);
                    if (this.life <= 0) KillIt();
                    callSignal(this.healthSignal, value);
                    break;
                }
            case ResourceType.mana:
                {
                    this.mana = Utilities.Resources.setResource(this.mana, this.maxMana, value);
                    if (showingDamageNumber && value > 0) showDamageNumber(value, GlobalValues.blue);
                    callSignal(this.manaSignal, value);
                    break;
                }
            case ResourceType.item:
                {
                    if (item != null)
                    {
                        Utilities.Items.updateInventory(item, this, Mathf.RoundToInt(value));
                        if (raiseResourceSignal) callSignal(this.currencies, value);
                    }
                    break;
                }
            case ResourceType.skill:
                {
                    if (item != null && item.skill != null && this.GetComponent<Player>() != null)
                    {
                        Utilities.Skill.updateSkillset(item.skill, this.GetComponent<Player>());
                    }
                    break;
                }
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
        float startSpeedInPercent = this.startSpeed / 100;
        float addNewSpeed = startSpeedInPercent * (addSpeed / 100);
        float changeSpeed = startSpeedInPercent + addNewSpeed;

        this.speed = changeSpeed * this.timeDistortion * this.speedMultiply;
        if (affectAnimation) this.animator.speed = this.speed / (this.startSpeed * this.speedMultiply / 100);
    }

    public void updateSpellSpeed(float addSpellSpeed)
    {
        this.spellspeed = ((this.startSpellSpeed / 100) + (addSpellSpeed / 100)) * this.timeDistortion;
    }

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion / 100);

        /* if (this.CompareTag("Player"))
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

    public void resetCast(StandardSkill skill)
    {
        if (skill != null)
        {
            if (!skill.keepHoldTimer) skill.holdTimer = 0;
            hideCastBarAndIndicator(skill);
        }
    }

    public void hideCastBarAndIndicator(StandardSkill skill)
    {
        if (this.activeCastbar != null)
        {
            this.activeCastbar.destroyIt();
        }

        skill.hideIndicator();
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
        if (this.canCollectAll || this.canCollect.Contains(item.itemGroup))
        {
            if (playSound) item.playSounds();

            bool playRaiseSound = false;
            if (item.itemGroup == "Kristalle") playRaiseSound = true;

            this.updateResource(playRaiseSound, item.resourceType, item, item.amount);

            if (destroyIt) item.DestroyIt();
        }
    }

    #endregion


    #region Damage Functions (hit, statuseffect, knockback)

    public void gotHit(StandardSkill skill, float percentage)
    {
        if ((!this.isInvincible && !this.isImmortal) || skill.ignoreInvincibility)
        {
            //Status Effekt hinzufügen
            if (skill.statusEffects != null)
            {
                foreach (StatusEffect effect in skill.statusEffects)
                {
                    Utilities.StatusEffectUtil.AddStatusEffect(effect, this);
                }
            }

            foreach (affectedResource elem in skill.affectedResources)
            {
                float amount = elem.amount * percentage / 100;

                updateResource(elem.resourceType, null, amount);

                if (this.life > 0 && elem.resourceType == ResourceType.life && amount < 0)
                {
                    AIAggroSystem aggro = this.GetComponent<AIAggroSystem>();
                    if (aggro != null) aggro.increaseAggroOnHit(skill.sender, elem.amount);

                    //Charakter-Treffer (Schaden) animieren
                    Utilities.Audio.playSoundEffect(this.audioSource, this.hitSoundEffect);
                    StartCoroutine(hitCo());
                }
            }

            if (this.life > 0)
            {
                //Rückstoß ermitteln
                float knockbackTrust = skill.thrust - antiKnockback;
                knockBack(skill.knockbackTime, knockbackTrust, skill);
            }
        }
    }

    public void gotHit(StandardSkill skill)
    {
        gotHit(skill, 100);
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

    public void knockBack(float knockTime, float thrust, StandardSkill attack)
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
        if (this.showHitcolor) this.addColor(this.hitColor);

        yield return new WaitForSeconds(this.cannotBeHitTime);
        this.resetColor(this.hitColor);
        this.isInvincible = false;
    }

    private IEnumerator immortalCo(float duration)
    {
        this.isImmortal = true;
        yield return new WaitForSeconds(this.cannotBeHitTime);
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
