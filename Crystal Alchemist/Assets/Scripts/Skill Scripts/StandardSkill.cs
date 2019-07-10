using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SkillType
{
    physical,
    magical,
    item
}

public class StandardSkill : MonoBehaviour
{
    #region Attribute

    [Required("Name muss gesetzt sein!")]
    [BoxGroup("Pflichtfelder")]
    [Tooltip("Name des Angriffs")]
    public string skillName;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [EnumToggleButtons]
    public SkillType category = SkillType.magical;

    [BoxGroup("Pflichtfelder")]
    public string animationTriggerName = "";

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Sortierung")]
    public int orderIndex = 10;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string skillDescription;

    [Space(10)]
    [BoxGroup("Pflichtfelder")]
    [Required]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public Rigidbody2D myRigidbody;

    [BoxGroup("Pflichtfelder")]
    [Required]
    public Animator animator;

    [BoxGroup("Pflichtfelder")]
    [Required]
    [SerializeField]
    private List<Collider2D> myColliders;

    [Space(10)]
    [BoxGroup("Pflichtfelder")]
    [Tooltip("Schatten")]
    [SerializeField]
    private SpriteRenderer shadow;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Schattenfarbe")]
    [ShowIf("shadow")]
    [SerializeField]
    private Color shadowColor = new Color(0,0,0,40);

    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Castzeit bis zur Instanziierung (für Außen)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cast = 0;

    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Soll die Castzeit gespeichert bleiben. True = ja. Ansonsten reset bei Abbruch ")]
    public bool keepHoldTimer = false;

    [Space(10)]
    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Verzögerung bis Aktivierung")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float delay = 0;

    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Dauer des Angriffs. 0 = Animation, max bis Trigger")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float duration = 1;

    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    public bool deactivateByButtonUp = false;

    [Space(10)]
    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Abklingzeit nach Aktivierung (für Außen)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cooldown = 1;

    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Abklingzeit nach Kombo")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cooldownAfterCombo = 0;

    [FoldoutGroup("Zeit-Attribute", expanded: false)]
    [Tooltip("Zeit für eine Kombo")]
    [Range(0, Utilities.maxFloatSmall)]
    public float durationCombo = 0;


    ////////////////////////////////////////////////////////////////

    [TabGroup("Ziel Attribute")]
    [Tooltip("Veränderung des Ziels. Negativ = Schaden, Positiv = Heilung")]
    public List<affectedResource> affectedResources;

    [TabGroup("Ziel Attribute")]
    [Tooltip("Statuseffekte")]
    public List<StatusEffect> statusEffects;

    ////////////////////////////////////////////////////////////////

    [Space(10)]
    [TabGroup("Ziel Attribute")]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Stärke des Knockbacks")]
    public float thrust = 4;

    [TabGroup("Ziel Attribute")]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("thrust", 0f)]
    public float knockbackTime = 0.2f;

    ////////////////////////////////////////////////////////////////

    [TabGroup("Sender Attribute")]
    [EnumToggleButtons]
    [Tooltip("Art der Resource")]
    public ResourceType resourceType = ResourceType.mana;

    [TabGroup("Sender Attribute")]
    [ShowIf("resourceType", ResourceType.item)]
    [Tooltip("Benötigtes Item")]
    public Item item;

    [TabGroup("Sender Attribute")]
    [HideIf("resourceType", ResourceType.none)]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    [Tooltip("Höhe der Resource des Senders. Negativ = Schaden, Positiv = Heilung")]
    public float addResourceSender = 0;

    [TabGroup("Sender Attribute")]
    [HideIf("resourceType", ResourceType.none)]
    [Range(0, Utilities.maxFloatInfinite)]
    [Tooltip("Intervall während der Dauer des Skills Leben oder Mana verändert werden.")]
    public float intervallSender = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Bewegungsgeschwindigkeit während eines Casts")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float speedDuringCasting = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Bewegungsgeschwindigkeit während des Angriffs")]
    [Range(-Utilities.maxFloatSmall, Utilities.maxFloatSmall)]
    public float speedDuringDuration = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Soll die Geschwindigkeit auch die Animation beeinflussen?")]
    public bool affectAnimation = true;

    [TabGroup("Sender Attribute")]
    [Tooltip("Soll der Charakter während des Schießens weiterhin in die gleiche Richtung schauen?")]
    public bool lockMovementonDuration = false;

    //////////////////////////////////////////////////////////////////////////////

    [Space(10)]
    [TabGroup("Sender Attribute")]
    [Tooltip("True = nach vorne, False = Knockback")]
    [SerializeField]
    private bool forward = false;

    [TabGroup("Sender Attribute")]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Stärke des Knockbacks")]
    [SerializeField]
    private float selfThrust = 0;

    [TabGroup("Sender Attribute")]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("selfThrust", 0f)]
    [SerializeField]
    private float selfThrustTime = 0;


    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Kann der Knopf gedrückt gehalten werden für weitere Schüsse?")]
    public bool isRapidFire = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Ist das Projektil stationär. True = liegt einfach herum (z.B. Bombe)")]
    public bool isStationary = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Schattencollider")]
    [Range(-1, 0)]
    public float colliderHeightOffset = 0;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Geschwindigkeit des Projektils")]
    [Range(0, Utilities.maxFloatSmall)]
    public float speed = 0;

    [Space(10)]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    public bool rotateIt = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    [HideIf("rotateIt")]
    [SerializeField]
    private bool blendTree = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    [HideIf("rotateIt")]
    [SerializeField]
    private bool useOffSetToBlendTree = false;

    [ShowIf("rotateIt")]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Range(-360, 360)]
    public float rotationModifier = 0;

    [ShowIf("rotateIt")]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Soll der Hit-Sprite passend zur Flugbahn rotieren?")]
    public bool rotateEndSprite = false;

    [ShowIf("rotateIt")]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
    [Range(0, 90)]
    public float snapRotationInDegrees = 0f;

    [Space(10)]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Range(0, 2)]
    [Tooltip("Positions-Offset, damit es nicht im Character anfängt")]
    public float positionOffset = 1f;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Range(0, 2)]
    [Tooltip("Positions-Höhe")]
    public float positionHeight = 0f;

    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [Tooltip("Ob die Zielhilfe aktiviert werden soll")]
    public bool activeTargetHelper = false;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [Tooltip("Ob das Ziel erfasst werden soll. Wenn NULL, dann nicht.")]
    public GameObject lockOn;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Tooltip("Manual = Spieler kann Ziele auswählen, Single = näheste Ziel, Multi = Alle in Range, Auto = Sofort ohne Bestätigung")]
    [EnumToggleButtons]
    public TargetingMode targetingMode = TargetingMode.manual;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Range(0, Utilities.maxIntSmall)]
    public int maxAmountOfTargets = 1;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float targetingDuration = 6f;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
    [Range(0, 10)]
    public float multiHitDelay = 0;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showRange = false;

    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt nur auf sich selbst")]
    public bool affectSelf = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectPlayers = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectEnemies = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Objekte")]
    public bool affectObjects = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Skills")]
    public bool affectSkills = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("Unverwundbarkeit ignorieren (z.B. für Heals)?")]
    public bool ignoreInvincibility = false;



    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [Range(1, Utilities.maxIntInfinite)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe")]
    public int maxAmounts = Utilities.maxIntInfinite;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe in einer Combo")]
    public int comboAmount = Utilities.maxIntSmall;

    //private bool showIndicator = false;
    //private bool showCastBar = false;

    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Sound und Icons", expanded: false)]
    [Tooltip("Icon für den Spieler")]
    public Sprite icon;

    [FoldoutGroup("Sound und Icons", expanded: false)]
    public AudioClip startSoundEffect;

    [FoldoutGroup("Sound und Icons", expanded: false)]
    public AudioClip endSoundEffect;

    [FoldoutGroup("Sound und Icons", expanded: false)]
    public AudioClip animSoundeffect;

    ////////////////////////////////////////////////////////////////

    private bool playStartEffectAlready = false;
    private Vector2 tempVelocity;
    private float elapsed;
    

    [HideInInspector]
    public Character sender;
    [HideInInspector]
    public float delayTimeLeft;
    [HideInInspector]
    public float durationTimeLeft;
    [HideInInspector]
    public float cooldownTimeLeft;



    [HideInInspector]
    public Character target;
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public bool playEndEffectAlready = false;


    [HideInInspector]
    public float timeDistortion = 1;
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool isActive = true; //damit kein Zeitstop bei einem stumpfen Pfeil gemacht wird
    [HideInInspector]
    public float holdTimer = 0;
    [HideInInspector]
    public bool triggerIsActive = true;
    [HideInInspector]
    public bool setPositionAtStart = true;

    #endregion


    #region Start Funktionen (Init, set Basics, Update Sender, set Position

    public void Start()
    {
        init();
    }

    public virtual void init()
    {
        this.cooldownTimeLeft = 0;
        setBasicAttributes();
        updateResourceSender();
        setSelfTrust();

        if (this.sender == null)
        {
            throw new System.Exception("No SENDER found! Must be player!");
        }

        

        //this.gameObject.layer = LayerMask.NameToLayer(this.sender.gameObject.tag + " Skill");        
    }

    private void setSelfTrust()
    {
        if (this.selfThrust > 0)
        {
            this.duration = this.selfThrustTime;
            int trustdirection = -1; //knockback
            if (forward) trustdirection = 1; //dash

            this.sender.knockBack(selfThrustTime, selfThrust, (this.sender.direction * trustdirection));
        }
    }

    private void setBasicAttributes()
    {
        //Setze Basis-Attribute, damit auch alles funktioniert

        this.name = this.skillName + Time.deltaTime;

        if (this.myRigidbody == null) this.myRigidbody = GetComponent<Rigidbody2D>();
        if(this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.animator == null) this.animator = GetComponent<Animator>();

        if (this.shadow != null && this.spriteRenderer != null)
        {
            this.shadow.sprite = this.spriteRenderer.sprite;
            this.shadow.color = this.shadowColor;
        }

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;        

        this.delayTimeLeft = this.delay;
        this.durationTimeLeft = this.duration;

        this.elapsed = this.intervallSender;

        if (this.startSoundEffect != null) Utilities.playSoundEffect(this.audioSource, this.startSoundEffect);

        if (this.sender == null)
        {
            this.sender = this.transform.parent.GetComponent<Character>(); //SET SENDER IF NULL (IMPORTANT!)            
        }

        this.sender.startAttackAnimation(this.animationTriggerName);
    }

    private void updateResourceSender()
    {
        if (this.sender != null)
        {
            this.sender.updateResource(this.resourceType, this.item, this.addResourceSender);
            setPostionAndDirection();
        }
    }

    private void setPostionAndDirection()
    {
        //Bestimme Winkel und Position

        float angle;
        Vector2 start;
        Vector3 rotation;

        if (!this.blendTree)
        {
            Utilities.setDirectionAndRotation(this.sender.transform.position, this.sender.direction, this.target,
                                          this.positionOffset, this.positionHeight, this.snapRotationInDegrees, this.rotationModifier,
                                          out angle, out start, out this.direction, out rotation);

            //if (this.target != null) this.direction = (Vector2)this.target.transform.position - start;                       

            if (this.colliderHeightOffset != 0)
            {
                foreach (Collider2D collider in this.myColliders)
                {
                    this.setColliderOffset(collider);
                }
            }

            if (setPositionAtStart) this.transform.position = start;

            if (this.myRigidbody != null)
            {
                this.myRigidbody.velocity = this.direction.normalized * this.speed;
                this.tempVelocity = this.myRigidbody.velocity;
            }

            if (this.rotateIt) transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            if(this.useOffSetToBlendTree) this.transform.position = new Vector2(this.sender.transform.position.x + (this.sender.direction.x * positionOffset),
                                                                                this.sender.transform.position.y + (this.sender.direction.y * positionOffset) + positionHeight);            
        }

        if (this.animator != null)
        {
            Utilities.SetAnimatorParameter(this.animator, "moveX", this.sender.direction.x);
            Utilities.SetAnimatorParameter(this.animator, "moveY", this.sender.direction.y);
        }

        if (this.shadow != null)
        {
            this.shadow.transform.position = (Vector2)this.transform.position - (setColliderOffset());
        }
    }

    #endregion

    private Vector2 setColliderOffset()
    {
        return new Vector2(-Mathf.Abs(this.direction.y) * (this.colliderHeightOffset / this.transform.localScale.y),
                           -Mathf.Abs(this.direction.x) * (this.colliderHeightOffset / this.transform.localScale.x));
    }


    private Vector2 setColliderOffset(Vector2 direction)
    {
        return new Vector2(direction.y * (this.colliderHeightOffset / this.transform.localScale.y),
                           direction.x * (this.colliderHeightOffset / this.transform.localScale.x));        
    }

    private void setColliderOffset(Collider2D collider)
    {
        if (collider != null) collider.offset = setColliderOffset(this.direction);
    }


    #region Update Funktionen   

    public void Update()
    {
        doOnUpdate();
        //Debug.Log(this.transform.position +": "+this.shadow.transform.position);
    }

    public virtual void doOnUpdate()
    {
        if (this.intervallSender > 0)
        {
            if (this.elapsed > 0) this.elapsed -= (Time.deltaTime * this.timeDistortion);
            else
            {
                if (this.sender != null)
                {
                    if (this.sender.getResource(this.resourceType, this.item) + this.addResourceSender < 0)
                        this.durationTimeLeft = 0;
                    else
                    {
                        this.elapsed = this.intervallSender;
                        this.sender.updateResource(this.resourceType, this.item, this.addResourceSender);
                    }
                }
            }
        }

        if (this.speedDuringDuration != 0) this.sender.updateSpeed(this.speedDuringDuration, this.affectAnimation);

        if (this.animator != null && this.sender != null && !this.lockMovementonDuration)
        {
            Utilities.SetAnimatorParameter(this.animator, "moveX", this.sender.direction.x);
            Utilities.SetAnimatorParameter(this.animator, "moveY", this.sender.direction.y);
        }

        if (this.delayTimeLeft > 0)
        {
            this.delayTimeLeft -= (Time.deltaTime * this.timeDistortion);

            Utilities.SetAnimatorParameter(this.animator, "Time", this.delayTimeLeft);
            //do something here
        }
        else
        {
            //Prüfe ob der Skill eine Maximale Dauer hat
            if (this.durationTimeLeft < Utilities.maxFloatInfinite)
            {
                if (this.durationTimeLeft > 0)
                {
                    this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);
                }
                else
                {
                    Utilities.SetAnimatorParameter(this.animator, "Explode", true);

                    if (this.animator == null || !Utilities.HasParameter(this.animator, "Explode")) DestroyIt();
                }
            }
        }

        if (this.target != null && !this.target.gameObject.activeInHierarchy) this.durationTimeLeft = 0;

    }

    public void landAttack(Character hittedObject)
    {
        if (hittedObject != null)
        {
            //Gegner zurückstoßen + Hit
            hittedObject.gotHit(this);
        }
    }

    public void landAttack(Collider2D hittedObject)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            //Gegner zurückstoßen + Hit
            landAttack(hittedObject.GetComponent<Character>());
        }
    }

    #endregion


    #region Trigger

    public virtual void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        if (Utilities.checkCollision(hittedCharacter, this)) landAttack(hittedCharacter);
    }

    public virtual void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (Utilities.checkCollision(hittedCharacter, this)) landAttack(hittedCharacter);
    }

    public virtual void OnTriggerExit2D(Collider2D hittedCharacter)
    {

    }

    #endregion


    #region AnimatorEvents

    public void PlayStartSoundEffect()
    {
        if (!this.playStartEffectAlready)
        {
            playSoundEffect(this.startSoundEffect);
            this.playStartEffectAlready = true;
        }
    }

    public void PlayAnimatorSoundEffect()
    {
        playSoundEffect(this.animSoundeffect);
    }

    public void PlayEndSoundEffect()
    {
        if (!this.playEndEffectAlready)
        {
            playSoundEffect(this.endSoundEffect);
            this.playEndEffectAlready = true;
        }
    }

    private void playSoundEffect(AudioClip clip)
    {
        if (clip != null) Utilities.playSoundEffect(this.audioSource, clip);
    }

    public void ActivateIt()
    {
        this.delayTimeLeft = 0;
    }

    public void SetTriggerActive(int value)
    {
        if (value == 0) this.triggerIsActive = false;
        else this.triggerIsActive = true;
    }

    public virtual void DestroyIt()
    {
        if (this.speedDuringDuration != 0) this.sender.updateSpeed(0);

        this.sender.activeSkills.Remove(this);
        //this.isActive = false;
        Destroy(this.gameObject);
    }

    #endregion


    #region Update Extern

    public void updateTimeDistortion(float distortion) //Signal?
    {
        this.timeDistortion = 1 + (distortion / 100);

        if (this.animator != null) this.animator.speed = this.timeDistortion;
        if (this.audioSource != null) this.audioSource.pitch = this.timeDistortion;
        if (this.myRigidbody != null && this.isActive) this.myRigidbody.velocity = this.direction.normalized * this.speed * this.timeDistortion;
    }

    #endregion

}

