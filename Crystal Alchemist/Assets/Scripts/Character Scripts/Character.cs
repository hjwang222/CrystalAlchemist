using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    dead
}

public enum CharacterType
{
    Player,
    Enemy,
    NPC,
    Object
}

public enum Element
{
    fire,
    water,
    earth,
    wind,
    thunder,
    ice,
    light,
    darkness,
    none
}

public enum Gender
{
    male,
    female,
    none
}

public enum DeathType
{
    destroy,
    //respawn,
    //rest,
    immortal
}
#endregion

#region Objects
[System.Serializable]
public struct LootTable
{
    public GameObject item;

    [Range(0, 100)]
    public int dropRate;
}
#endregion

public class Character : MonoBehaviour
{

    #region Basic Attributes
    [Header("Character Information")]
    [Tooltip("Name")]
    public string characterName;
    [Tooltip("Rasse")]
    public string characterSpecies;
    [Tooltip("Geschlecht")]
    public Gender characterGender = Gender.none;
    [Tooltip("Um welchen Typ handelt es sich?")]
    public CharacterType characterType = CharacterType.Object;
    [Tooltip("Elementar des Charakters")]
    public Element element = Element.none;

    [Header("Character Stats")]
    [Tooltip("Leben, mit dem der Spieler startet")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float startLife;
    [Tooltip("Mana, mit dem der Spieler startet")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float startMana;
    [Tooltip("Movement-Speed in %, mit dem der Spieler startet")]
    [Range(Utilities.minFloatPercent, Utilities.maxFloatPercent)]
    public float startSpeed = 100;
    [Tooltip("Geschwindigkeitsmodifier in % von Cooldown und Castzeit")]
    [Range(Utilities.minFloatPercent, Utilities.maxFloatPercent)]
    public float startSpellSpeed = 100;
    [Tooltip("Immunität von Statuseffekten")]
    public List<StatusEffect> immunityToStatusEffects = new List<StatusEffect>();


    [Header("Skills")]
    [Tooltip("Skills, welcher der Character verwenden kann")]
    public List<Skill> skills = new List<Skill>();
    [Tooltip("Skill, welcher der Character sofort verwendet")]
    public Skill initializeSkill;
    [Tooltip("Skill, welcher der Character bei seinem Tod verwendet")]
    public Skill deathSkill;



    [Header("Character Regeneration")]
    [Tooltip("Höhe der Lebensregeneration")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float lifeRegeneration = 0;
    [Tooltip("Intervall der Lebensregeneration")]
    [Range(0, Utilities.maxFloatSmall)]
    public float lifeRegenerationInterval = 0;
    [Tooltip("Höhe der Manaregeneration")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float manaRegeneration = 0;
    [Tooltip("Intervall der Manaregeneration")]
    [Range(0, Utilities.maxFloatSmall)]
    public float manaRegenerationInterval = 0;

    [Header("Character Max Values")]
    [Tooltip("Maximales Life")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float attributeMaxLife;
    [Tooltip("Maximales Mana")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float attributeMaxMana;

    [Header("Damage Behavior")]
    [Tooltip("Wie stark (-) oder schwach (+) kann das Objekt zurück gestoßen werden?")]
    [Range(-Utilities.maxFloatSmall, Utilities.maxFloatSmall)]
    public float antiKnockback = 0;
    [Tooltip("Unverwundbarkeitszeit")]
    [Range(0, 10)]
    public float cannotBeHitTime = 0.3f;
    [Tooltip("Kann das Objekt berührt werden?")]
    public bool isTouchable = true;
    [Tooltip("Zusätzlicher Effekt bei Tod, ansonsten Animator benutzen")]
    public GameObject DeathEffect;

    [Header("Character Inventory")]
    [Range(0, Utilities.maxFloatInfinite)]
    public int coins;
    [Range(0, Utilities.maxFloatInfinite)]
    public int crystals;
    [Range(0, Utilities.maxFloatInfinite)]
    public int keys;

    [Header("Loot")]
    [Tooltip("Items und deren Wahrscheinlichkeit zwischen 1 und 100")]
    public LootTable[] lootTable;
    [Tooltip("Multiloot = alle Items. Ansonsten nur das seltenste Item")]
    public bool multiLoot = false;

    [Header("Objekt-Values")]
    [Tooltip("Art des Todes. Rest, wenn Überreste. Destroy, ohne Respawn. Respawn.")]
    public DeathType deathType = DeathType.destroy;
    [Tooltip("Farbe, wenn Gegner getroffen wurde")]
    public bool showHitcolor = true;
    public Color hitColor = Color.white;
    [Tooltip("Dialog-Texte als Liste")]
    public List<string> dialog = new List<string>();
    [Tooltip("Context-Objekt hier rein (nur für Interagierbare Objekte)")]
    public GameObject contextClueChild;
    [Tooltip("DamageNumber-Objekt hier rein (nur für zerstörbare Objekte)")]
    public GameObject damageNumber;

    [Header("Sound Effects")]
    [Tooltip("Soundeffekt, wenn Gegner getroffen wurde")]
    public AudioClip hitSoundEffect;
    [Tooltip("Soundeffekt, wenn Gegner getötet wurde")]
    public AudioClip killSoundEffect;
    [Tooltip("Scriptable Object für die Lautstärke der Effekte")]
    public FloatValue soundEffectVolume;

    #endregion
    
    #region Attributes

    [Header("Charakter Attribute (Scriptable Object)")]
    [Tooltip("Pflichtfeld! Ohne Attribute funktioniert es nicht!")]
    public Globals global;

    [Header("Signale")]
    [Tooltip("GUI Update Signal für Life")]
    public Signal healthSignal;
    [Tooltip("GUI Update Signal für Mana")]
    public Signal manaSignal;

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
    public Transform homePosition;
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
    public List<GameObject> items = new List<GameObject>();
    [HideInInspector]
    public List<StatusEffect> buffs = new List<StatusEffect>();
    [HideInInspector]
    public List<StatusEffect> debuffs = new List<StatusEffect>();
    [HideInInspector]
    public List<Skill> activeSkills = new List<Skill>();
    [HideInInspector]
    private float lifeTime;
    [HideInInspector]
    private float manaTime;

    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool lockAnimation = false;
    private bool killOnce = false;
    [HideInInspector]
    public float timeDistortion = 1;
    private float speedMultiply = 5;
    public GameObject activeLockOnTarget = null;

    #endregion


    #region Start Functions (Spawn, Init)
    void Start()
    {
        init();
    }

    public void init()
    {
        this.direction = new Vector2(0, -1);
        //getItems();    
        
        setComponents();
        spawn();
        Utilities.setItem(this.lootTable, this.multiLoot, this.items);
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
        if (this.spriteRenderer != null) this.spriteRenderer.color = this.global.color;
        this.animator = GetComponent<Animator>();
        this.transform.gameObject.tag = this.characterType.ToString();
    }

    public void spawn()
    {        
        this.life = this.startLife;
        this.mana = this.startMana;
        this.speed = (this.startSpeed*this.speedMultiply / 100);
        this.spellspeed = (this.startSpellSpeed / 100);
        
        this.currentState = CharacterState.idle;
        this.animator.enabled = true;
        this.spriteRenderer.enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
        if (this.homePosition != null) this.transform.position = this.homePosition.position;

    }
    #endregion


    private void Update()
    {
        regeneration();        
    }

    public void regeneration()
    {
        if (this.lifeRegeneration != 0 && this.lifeRegenerationInterval != 0)
        {
            if (this.lifeTime >= this.lifeRegenerationInterval)
            {
                this.lifeTime = 0;
                updateLife(this.lifeRegeneration);
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
                updateMana(this.manaRegeneration);
            }
            else
            {
                this.manaTime += (Time.deltaTime * this.timeDistortion);
            }
        }
    }

    private void useSkillInstantly(Skill skill)
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
                && this.mana + skill.addManaSender >= 0
                && this.life + skill.addLifeSender > 0)
            {
                    if (!skill.isRapidFire && !skill.keepHoldTimer) skill.holdTimer = 0;
                    
                    skill.cooldownTimeLeft = skill.cooldown; //Reset cooldown

                    if (skill.isStationary)
                    {
                    //Place it in World 
                        GameObject activeSkill = Instantiate(skill.gameObject, this.transform.position, Quaternion.identity);
                        activeSkill.GetComponent<Skill>().sender = this;
                        this.activeSkills.Add(activeSkill.GetComponent<Skill>());
                    }
                    else
                    {
                    //Place it as Child
                        GameObject activeSkill = Instantiate(skill.gameObject, this.transform.position, Quaternion.identity, this.transform);
                        activeSkill.GetComponent<Skill>().sender = this;
                        this.activeSkills.Add(activeSkill.GetComponent<Skill>());
                    }                
            }
        }
    }




    #region Day/Night Circle  

    public void updateColor()
    {
        if (this.spriteRenderer != null) this.spriteRenderer.color = this.global.color;
    }

    #endregion


    #region SkillUsage


    #region Utils

    public int getAmountOfSameSkills(Skill skill)
    {
        int result = 0;

        for (int i = 0; i < this.activeSkills.Count; i++)
        {
            Skill activeSkill = this.activeSkills[i];
            if (activeSkill.skillName == skill.skillName) result++;
        }

        return result;
    }
    #endregion



    #endregion



    #region Item Functions (drop Item, Lootregeln)

    public void dropItem()
    {
        foreach (GameObject itemObject in this.items)
        {
            GameObject itemClone = Instantiate(itemObject, this.transform.position, Quaternion.identity);
        }
    }

    #endregion


    #region Update Functions (Signals?)  

    //Signal?

    public void updateLife(float addLife)
    {
        if (addLife != 0)
        {
            if (this.life + addLife > this.attributeMaxLife) addLife = 0;
            this.life += addLife;

            if (this.healthSignal != null) this.healthSignal.Raise();

            if (this.damageNumber != null)
            {
                GameObject damageNumberClone = Instantiate(this.damageNumber, this.transform.position, Quaternion.identity, this.transform);
                damageNumberClone.GetComponent<DamageNumbers>().number = addLife;
                damageNumberClone.hideFlags = HideFlags.HideInHierarchy;
            }

            if (this.life <= 0)
            {
                //Charakter töten
                StartCoroutine(killCo());
            }
        }
    }

    public void updateMana(float addMana)
    {
        if (addMana != 0)
        {
            if (this.mana + addMana > this.attributeMaxMana) addMana = 0;
            else if (this.mana + addMana < 0) this.mana = 0;
            else this.mana += addMana;

            if (this.manaSignal != null) this.manaSignal.Raise();
        }
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


    #region Damage Functions (hit, statuseffect, knockback)

    public void gotHit(Skill skill)
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

                if (skill.addLifeTarget < 0)
                {
                    //Charakter-Treffer (Schaden) animieren
                    Utilities.playSoundEffect(this.audioSource, this.hitSoundEffect, this.soundEffectVolume);
                    StartCoroutine(hitCo());
                }

                updateLife(skill.addLifeTarget);

                if(this.life > 0)
                {
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
            effect.destroy();
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
                    GameObject statusEffectClone = Instantiate(statusEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
                    StatusEffect statusEffectScript = statusEffectClone.GetComponent<StatusEffect>();
                    statusEffectScript.target = this;
                    statusEffectClone.hideFlags = HideFlags.HideInHierarchy;

                    //add to list for better reference
                    statusEffects.Add(statusEffectClone.GetComponent<StatusEffect>());
                }
                else
                {
                    if (statusEffect.canOverride && statusEffect.endType == StatusEffectEndType.time)
                    {
                        //Wenn der Effekt überschreiben kann, soll der Effekt mit der kürzesten Dauer entfernt werden
                        StatusEffect toDestroy = result[0];
                        toDestroy.destroy();

                        GameObject statusEffectClone = Instantiate(statusEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
                        StatusEffect statusEffectScript = statusEffectClone.GetComponent<StatusEffect>();
                        statusEffectScript.target = this;
                        statusEffectClone.hideFlags = HideFlags.HideInHierarchy;

                        //add to list for better reference
                        statusEffects.Add(statusEffectClone.GetComponent<StatusEffect>());
                    }
                    else if (statusEffect.canDeactivateIt && statusEffect.endType == StatusEffectEndType.mana)
                    {
                        StatusEffect toDestroy = result[0];
                        toDestroy.destroy();
                    }
                }
            }       
        }
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
        this.spriteRenderer.color = this.global.color;
    }

    public IEnumerator killCo()
    {
        if (!this.killOnce)
        {
            this.killOnce = true;
            if (this.myRigidbody != null) this.myRigidbody.velocity = Vector2.zero;

            Utilities.playSoundEffect(this.audioSource, this.killSoundEffect, this.soundEffectVolume);

            this.currentState = CharacterState.dead;
            
            if (this.deathType != DeathType.immortal)
            {
                if (this.animator != null) this.animator.SetBool("isDead", true);
                               
                this.GetComponent<BoxCollider2D>().enabled = false;
                
                if (this.DeathEffect != null)
                {
                    //Todes-Effekt abspielen und anschließend zerstören
                    GameObject effect = Instantiate(this.DeathEffect, transform.position, Quaternion.identity, this.transform);
                    Destroy(effect, 0.5f);
                }

                //Drop Item
                dropItem();
                yield return new WaitForSeconds(0.5f);

                if (this.deathType == DeathType.destroy)
                {
                    //zerstöre das Objekt für immer
                    Destroy(this.gameObject);
                }               
            }
        }
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
