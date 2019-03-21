using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    #region Attribute

    [Header("Identifizierung")]
    [Tooltip("Name des Angriffs")]
    public string skillName;
    public Globals global;
    public FloatValue soundEffectVolume;

    [Header("Zeit-Attribute")]
    [Tooltip("Castzeit bis zur Instanziierung (für Außen)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cast = 0;
    [Tooltip("Soll die Castzeit gespeichert bleiben. True = ja. Ansonsten reset bei Abbruch ")]
    public bool keepHoldTimer = false;
    [Tooltip("Verzögerung bis Aktivierung")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float delay = 0;
    [Tooltip("Zeitpunkt der Scriptaktivierung")]
    [Range(0, Utilities.maxFloatSmall)]
    public float scriptActivation = 0;
    [Tooltip("Dauer des Angriffs. 0 = Animation, max bis Trigger")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float duration = 1;
    [Tooltip("Abklingzeit nach Aktivierung (für Außen)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cooldown = 1;
    [Tooltip("Abklingzeit nach Kombo")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cooldownAfterCombo = 0;
    [Tooltip("Zeit für eine Kombo")]
    [Range(0, Utilities.maxFloatSmall)]
    public float durationCombo = 0;

    [Header("Zielerfassungsattribute")]
    [Tooltip("Ob das Ziel erfasst werden soll. Wenn NULL, dann nicht.")]
    public GameObject lockOn;
    [Range(0, Utilities.maxIntSmall)]
    public int maxAmountOfTargets = 1;
    [Range(0, Utilities.maxFloatInfinite)]
    public float targetingDuration = 6f;
    [Tooltip("Manual = Spieler kann Ziele auswählen, Single = näheste Ziel, Multi = Alle in Range, Auto = Sofort ohne Bestätigung")]
    public TargetingMode targetingMode = TargetingMode.manual;
    [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
    [Range(0, 10)]
    public float multiHitDelay = 0;
    public bool showRange = false;

    [Header("Basis Attribute (bezogen auf Effekte des Ziels)")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    [Tooltip("Lebensveränderung des Ziels. Negativ = Schaden, Positiv = Heilung")]
    public float addLifeTarget = -1;
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    [Tooltip("Manaveränderung des Ziels. Negativ = Schaden, Positiv = Heilung")]
    public float addManaTarget = 0;
    [Tooltip("Statuseffekte")]
    public List<StatusEffect> statusEffects;
    public Character target;

    [Header("Basis Attribute (bezogen auf Effekte des Senders)")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    [Tooltip("Manaveränderung des Senders. Negativ = Schaden, Positiv = Heilung")]
    public float addLifeSender = 0;
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    [Tooltip("Manaveränderung des Senders. Negativ = Schaden, Positiv = Heilung")]
    public float addManaSender = 0;
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Intervall während der Dauer des Skills Leben oder Mana verändert werden.")]
    public float intervallSender = 0;
    [Tooltip("Bewegungsgeschwindigkeit während eines Casts")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float speedDuringCasting = 0;
    [Tooltip("Bewegungsgeschwindigkeit während des Angriffs")]
    [Range(-Utilities.maxFloatSmall, Utilities.maxFloatSmall)]
    public float speedDuringDuration = 0;
    [Tooltip("Soll der Charakter während des Schießens weiterhin in die gleiche Richtung schauen?")]
    public bool lockMovementonDuration = false;

    [Header("Projektil Attribute")]
    [Tooltip("Geschwindigkeit des Projektils")]
    [Range(0, Utilities.maxFloatSmall)]
    public float speed = 0;
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    public bool rotateIt = false;
    [Tooltip("Soll der Hit-Sprite passend zur Flugbahn rotieren?")]
    public bool rotateEndSprite = false;
    [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
    [Range(0, 90)]
    public float snapRotationInDegrees = 0f;
    [Tooltip("Kann der Knopf gedrückt gehalten werden für weitere Schüsse?")]
    public bool isRapidFire = false;
    [Range(0, 2)]
    [Tooltip("Positions-Offset, damit es nicht im Character anfängt")]
    public float positionOffset = 1f;
    [Range(0, 2)]
    [Tooltip("Positions-Höhe")]
    public float positionHeight = 0f;
    [Tooltip("Ist das Projektil stationär. True = liegt einfach herum (z.B. Bombe)")]
    public bool isStationary = false;

    [Header("Wirkungsbereich")]
    [Tooltip("wirkt nur auf sich selbst")]
    public bool affectSelf = false;
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectPlayers = false;
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectEnemies = false;
    [Tooltip("wirkt auf alle Objekte")]
    public bool affectObjects = false;
    [Tooltip("wirkt auf alle Skills")]
    public bool affectSkills = false;
    [Tooltip("Unverwundbarkeit ignorieren (z.B. für Heals)?")]
    public bool ignoreInvincibility = false;

    [Header("Knockback-Attribute")]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Stärke des Knockbacks")]
    public float thrust = 4;
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Dauer des Knockbacks")]
    public float knockbackTime = 0.2f;

    [Header("Special Behaviors")]
    [Range(1, Utilities.maxIntInfinite)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe")]
    public int maxAmounts = Utilities.maxIntInfinite;
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe in einer Combo")]
    public int comboAmount = Utilities.maxIntSmall;
    [Tooltip("Beschwörungen")]
    public GameObject summon;
    [Tooltip("Hier kann ein zusätzliches Script rein. Normalerweise wird es automatisch hinzugefügt, sofern vorhanden")]
    public Script script;

    [Header("RPG-Attribute")]
    [Tooltip("Elementar des Skills")]
    public Element element = Element.none;
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string skillDescription;
    [Tooltip("Von wem stammt der Skill?")]
    public Character sender;
    [Tooltip("Icon für den Spieler")]
    public Sprite icon;

    public AudioClip startSoundEffect;
    private bool playStartEffectAlready = false;
    public AudioClip endSoundEffect;
    public AudioClip animSoundeffect;

    public bool showIndicator = false;
    public bool showCastBar = false;

    [HideInInspector]
    public float delayTimeLeft;
    [HideInInspector]
    public float durationTimeLeft;
    [HideInInspector]
    public float scriptActivationTimeLeft;
    [HideInInspector]
    public float cooldownTimeLeft;

    [HideInInspector]
    public Rigidbody2D myRigidbody;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public bool playEndEffectAlready = false;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    private Vector2 tempVelocity;

    [HideInInspector]
    public float timeDistortion = 1;
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool isNotActive = false; //damit kein Zeitstop bei einem stumpfen Pfeil gemacht wird
    private float elapsed;
    [HideInInspector]
    public float holdTimer = 0;

    #endregion


    #region Start Funktionen (Init, set Basics, Update Sender, set Position

    private void Start()
    {
        init();        
    }

    private void init()
    {
        this.cooldownTimeLeft = 0;
        setBasicAttributes();
        updateLifeManaFromSender();        

        this.script = Utilities.setScript(this.GetComponent<Script>(), this.transform, this, this.target);
        
        if(this.sender == null)
        {
            throw new System.Exception("No SENDER found! Must be player!");
        }

        //this.gameObject.layer = LayerMask.NameToLayer(this.sender.gameObject.tag + " Skill");

        if (this.script != null) this.script.onInitialize();
    }   

    private void setBasicAttributes()
    {
        //Setze Basis-Attribute, damit auch alles funktioniert

        this.name = this.skillName + Time.deltaTime;
        this.myRigidbody = GetComponent<Rigidbody2D>();
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.spriteRenderer != null && this.global != null) this.spriteRenderer.color = this.global.color;
        this.animator = GetComponent<Animator>();

        this.delayTimeLeft = this.delay;
        this.durationTimeLeft = this.duration;
        this.scriptActivationTimeLeft = this.scriptActivation;

        this.elapsed = this.intervallSender;

        if (this.startSoundEffect != null) Utilities.playSoundEffect(this.audioSource, this.startSoundEffect, this.soundEffectVolume);
        
        if (this.sender == null) this.sender = this.transform.parent.GetComponent<Character>(); //SET SENDER IF NULL (IMPORTANT!)
    }

    private void updateLifeManaFromSender()
    {
        if (this.sender != null)
        {
            this.sender.updateMana(this.addManaSender);
            this.sender.updateLife(this.addLifeSender);
            setPostionAndDirection();
        }
    }

    private void setPostionAndDirection()
    {
        if (this.animator != null)
        {
            this.animator.SetFloat("moveX", this.sender.direction.x);
            this.animator.SetFloat("moveY", this.sender.direction.y);
        }

        //TODO: AUSLAGERN!
        this.direction = this.sender.direction;
        

        Vector2 start = new Vector2(this.sender.transform.position.x + (this.sender.direction.x * this.positionOffset),
                                    this.sender.transform.position.y + (this.sender.direction.y * this.positionOffset) + this.positionHeight);

        if (this.target != null) this.direction = (Vector2)this.target.transform.position - start;

        this.transform.position = start;
        //Utilities.berechneWinkel(this.sender.transform.position, this.sender.direction, this.positionOffset, this.rotateIt, this.snapRotationInDegrees, out angle, out start, out this.direction);

        float angle = 0;
        if (this.rotateIt) angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;

        if (this.snapRotationInDegrees > 0)
        {
            angle = Mathf.Round(angle / this.snapRotationInDegrees) * this.snapRotationInDegrees;
            this.direction = DegreeToVector2(angle);
        }

        Vector3 rotation = new Vector3(0, 0, angle);

        if (this.myRigidbody != null)
        {
            this.myRigidbody.velocity = this.direction.normalized * this.speed;
            this.tempVelocity = this.myRigidbody.velocity;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    #endregion


    #region Update Funktionen   

    private void Update()
    {
        doOnUpdate();
    }

    private void doOnUpdate()
    {
        if (this.intervallSender > 0)
        {
            if (this.elapsed > 0) this.elapsed -= (Time.deltaTime * this.timeDistortion);
            else
            {
                if (this.sender != null)
                {
                    if (this.sender.mana - this.addManaSender < 0) this.duration = 0;
                    else
                    {
                        this.elapsed = this.intervallSender;
                        this.sender.updateMana(this.addManaSender);
                        this.sender.updateLife(this.addLifeSender);
                    }
                }
            }
        }

        if (this.speedDuringDuration != 0) this.sender.updateSpeed(this.speedDuringDuration);

        if (this.animator != null && this.sender != null && !this.lockMovementonDuration)
        {
            this.animator.SetFloat("moveX", this.sender.direction.x);
            this.animator.SetFloat("moveY", this.sender.direction.y);
        }

        if (this.delayTimeLeft > 0)
        {
            this.delayTimeLeft -= (Time.deltaTime * this.timeDistortion);
            if (this.animator != null) this.animator.SetFloat("Time", this.delayTimeLeft);
            //do something here
        }
        else
        {
            if (this.script != null)
            {
                if (this.scriptActivationTimeLeft > 0)
                {
                    this.scriptActivationTimeLeft -= (Time.deltaTime * this.timeDistortion);
                }
                else
                {
                    this.script.onUpdate();
                }
            }

            //Prüfe ob der Skill eine Maximale Dauer hat
            if (this.durationTimeLeft < Utilities.maxFloatInfinite)
            {
                if (this.durationTimeLeft > 0)
                {
                    this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);
                }
                else
                {
                    if (this.animator != null) this.animator.SetBool("Explode", true);

                    if (this.animator == null || this.animator.GetBool("Explode") == false) DestroyIt();
                }
            }
        }
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

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        if (Utilities.checkCollision(hittedCharacter, this)) landAttack(hittedCharacter);

        if (this.script != null)
        {            
            this.script.onStay(hittedCharacter);
        }
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (Utilities.checkCollision(hittedCharacter, this)) landAttack(hittedCharacter);
            
        if (this.script != null)
        {
            this.script.onEnter(hittedCharacter);
        }
    }

    private void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        if (this.script != null)
        {
            this.script.onExit(hittedCharacter);
        }
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
        if (clip != null) Utilities.playSoundEffect(this.audioSource, clip, this.soundEffectVolume);
    }

    public void ActivateIt()
    {
        this.delayTimeLeft = 0;
    }

    public void DestroyIt()
    {
        if (this.speedDuringDuration != 0) this.sender.updateSpeed(0);
        if (this.script != null) this.script.onDestroy();
        this.sender.activeSkills.Remove(this);
        Destroy(this.gameObject, 0.1f);
    }

    #endregion


    #region Update Extern

    public void updateTimeDistortion(float distortion) //Signal?
    {
        this.timeDistortion = 1 + (distortion/100);

        if (this.animator != null) this.animator.speed = this.timeDistortion;
        if (this.audioSource != null) this.audioSource.pitch = this.timeDistortion;
        if (this.myRigidbody != null && !this.isNotActive) this.myRigidbody.velocity = this.direction.normalized * this.speed * this.timeDistortion;    
    }

    #endregion


    #region Functions

    private Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    private Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    #endregion
}

