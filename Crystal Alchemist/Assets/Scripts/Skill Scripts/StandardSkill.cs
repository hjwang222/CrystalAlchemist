using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SkillType
{
    physical,
    magical,
    item
}

public enum StateType
{
    none,
    attack,
    defend
}

public class StandardSkill : MonoBehaviour
{
    #region Attribute

    [Required("Name muss gesetzt sein!")]
    [BoxGroup("Pflichtfelder")]
    [Tooltip("Name des Angriffs")]
    public string skillName;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Name des Angriffs")]
    public string skillNameEnglish;

    [Space(10)]
    [BoxGroup("Easy Access")]
    [Required]
    public SpriteRenderer spriteRenderer;

    [BoxGroup("Easy Access")]
    [Required]
    public Rigidbody2D myRigidbody;

    [BoxGroup("Easy Access")]
    [Required]
    public Animator animator;

    [Space(10)]
    [BoxGroup("Easy Access")]
    [Tooltip("Schatten")]
    public SpriteRenderer shadow;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Castzeit bis zur Instanziierung (für Außen)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cast = 0;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Soll die Castzeit gespeichert bleiben. True = ja. Ansonsten reset bei Abbruch ")]
    public bool keepHoldTimer = false;

    [Space(10)]
    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Verzögerung bis Aktivierung")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float delay = 0;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Dauer des Angriffs. 0 = Animation, max bis Trigger")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float duration = 1;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Soll der Charakter während des Schießens weiterhin in die gleiche Richtung schauen?")]
    [SerializeField]
    [Range(0, Utilities.maxFloatInfinite)]
    private float lockMovementonDuration = 0;

    [FoldoutGroup("Controls", expanded: false)]
    public bool deactivateByButtonUp = false;

    [FoldoutGroup("Controls", expanded: false)]
    public bool deactivateByButtonDown = false;

    [Space(10)]
    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Abklingzeit nach Aktivierung (für Außen)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cooldown = 1;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Abklingzeit nach Kombo")]
    [Range(0, Utilities.maxFloatSmall)]
    public float cooldownAfterCombo = 0;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Zeit für eine Kombo")]
    [Range(0, Utilities.maxFloatSmall)]
    public float durationCombo = 0;

    [FoldoutGroup("Controls", expanded: false)]
    [SerializeField]
    private bool canAffectedBytimeDistortion = true;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Kann der Knopf gedrückt gehalten werden für weitere Schüsse?")]
    public bool isRapidFire = false;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Ist das Projektil stationär. True = liegt einfach herum (z.B. Bombe)")]
    public bool isStationary = false;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Soll etwas während des Delays getan werden (DoCast Methode)")]
    public bool doCastDuringDelay = false;

    [FoldoutGroup("Controls", expanded: false)]
    [Range(1, Utilities.maxIntInfinite)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe")]
    public int maxAmounts = Utilities.maxIntInfinite;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe in einer Combo")]
    public int comboAmount = Utilities.maxIntSmall;


    ////////////////////////////////////////////////////////////////

    private bool playStartEffectAlready = false;

    [HideInInspector]
    public float elapsed;
    private float LockElapsed;




    [HideInInspector]
    public Character sender;
    [HideInInspector]
    public float delayTimeLeft;
    [HideInInspector]
    public float durationTimeLeft;
    [HideInInspector]
    public float cooldownTimeLeft;

    [HideInInspector]
    public bool movementLocked;

    [HideInInspector]
    public Character target;
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public bool dontPlayAudio = false;
    [HideInInspector]
    public bool basicRequirementsExists = true;
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
    public bool setActive = true;



    #endregion


    #region Start Funktionen (Init, set Basics, Update Sender, set Position

    public void preLoad()
    {
        PreLoadModule preLoadModule = this.GetComponent<PreLoadModule>();
        if (preLoadModule != null) preLoadModule.checkRequirements();
    }


    public void Start()
    {
        init();
    }

    public virtual void init()
    {
        //this.cooldownTimeLeft = 0;
        setBasicAttributes();

        if (this.sender == null)
        {
            throw new System.Exception("No SENDER found! Must be player!");
        }
        //this.gameObject.layer = LayerMask.NameToLayer(this.sender.gameObject.tag + " Skill");        
    }



    private void setBasicAttributes()
    {
        //Setze Basis-Attribute, damit auch alles funktioniert

        this.name = this.skillName + Time.deltaTime;

        if (this.myRigidbody == null) this.myRigidbody = GetComponent<Rigidbody2D>();
        if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.animator == null) this.animator = GetComponent<Animator>();

        if (this.shadow != null && this.spriteRenderer != null)
        {
            this.shadow.sprite = this.spriteRenderer.sprite;
        }

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        this.delayTimeLeft = this.delay;
        this.durationTimeLeft = this.duration;

        this.LockElapsed = this.lockMovementonDuration;
        if (this.LockElapsed > 0) this.movementLocked = true;

        if (this.sender == null)
        {
            this.sender = this.transform.parent.GetComponent<Character>(); //SET SENDER IF NULL (IMPORTANT!)            
        }
    }

    private void setTag(GameObject gameobject, string tag)
    {
        int childcount = gameobject.transform.childCount;
        gameobject.tag = tag;

        for (int i = 0; i < childcount; i++)
        {
            setTag(gameobject.transform.GetChild(i).gameObject, tag);
        }
    }





    #endregion


    #region Update Funktionen   

    public void Update()
    {
        doOnUpdate();
    }



    public float getDurationLeft()
    {
        return this.durationTimeLeft;
    }

    public virtual void doOnCast()
    {

    }

    public virtual void doOnUpdate()
    {


        if (this.LockElapsed > 0)
        {
            this.LockElapsed -= Time.deltaTime;
        }
        else
        {
            this.movementLocked = false;
        }

        if (this.spriteRenderer != null && this.shadow != null)
        {
            this.shadow.sprite = this.spriteRenderer.sprite;
        }





        if (this.animator != null && this.sender != null && !this.movementLocked)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", this.sender.direction.x);
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", this.sender.direction.y);
        }

        if (this.delayTimeLeft > 0)
        {
            SkillIndicatorModule indicatorModule = this.GetComponent<SkillIndicatorModule>();
            if (indicatorModule != null) indicatorModule.showIndicator();

            this.delayTimeLeft -= (Time.deltaTime * this.timeDistortion);

            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Time", this.delayTimeLeft);

            if (this.doCastDuringDelay) doOnCast();
        }
        else
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Active", true);
            SkillIndicatorModule indicatorModule = this.GetComponent<SkillIndicatorModule>();
            if (indicatorModule != null) indicatorModule.hideIndicator();

            SkillAnimationModule animationModule = this.GetComponent<SkillAnimationModule>();
            if (animationModule != null) animationModule.hideCastingAnimation();

            //Prüfe ob der Skill eine Maximale Dauer hat
            if (this.durationTimeLeft < Utilities.maxFloatInfinite)
            {
                if (this.durationTimeLeft > 0)
                {
                    this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);
                }
                else
                {
                    Utilities.UnityUtils.SetAnimatorParameter(this.animator, "Explode", true);

                    if (this.animator == null || !Utilities.UnityUtils.HasParameter(this.animator, "Explode"))
                    {
                        Debug.Log(this.skillName + " hat kein Animator oder Explode-Parameter");
                        SetTriggerActive(1);
                        DestroyIt();
                    }
                }
            }
        }

        if (this.target != null && !this.target.gameObject.activeInHierarchy) this.durationTimeLeft = 0;

    }



    public void landAttack(Collider2D hittedObject)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            //Gegner zurückstoßen + Hit
            hittedObject.GetComponent<Character>().gotHit(this);
        }
    }

    public void landAttack(Collider2D hittedObject, float percentage)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            //Gegner zurückstoßen + Hit
            hittedObject.GetComponent<Character>().gotHit(this, percentage);
        }
    }

    #endregion


    public bool isResourceEnough(Character character)
    {
        SkillSenderModule senderModule = this.GetComponent<SkillSenderModule>();

        if (senderModule == null) return true;
        else
        {
            if (character.getResource(senderModule.resourceType, senderModule.item) + senderModule.addResourceSender >= 0 //new method: Check if enough resource on skill
                           || senderModule.addResourceSender == -Utilities.maxFloatInfinite) return true;
            else return false;
        }
    }

    public float getSpeed()
    {
        if (this.GetComponent<SkillProjectile>() != null) return this.GetComponent<SkillProjectile>().speed;
        else return 0;
    }

    public bool rotateIt()
    {
        if (this.GetComponent<SkillTransformModule>() != null) return this.GetComponent<SkillTransformModule>().rotateIt;
        else return false;
    }


    #region Trigger

    public virtual void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this)) landAttack(hittedCharacter);
    }

    public virtual void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this)) landAttack(hittedCharacter);
    }

    public virtual void OnTriggerExit2D(Collider2D hittedCharacter)
    {

    }

    #endregion


    #region AnimatorEvents

    public void PlaySoundEffect(AudioClip audioClip)
    {
        Utilities.Audio.playSoundEffect(this.audioSource, audioClip);
    }

    public void PlaySoundEffectOnce(AudioClip audioClip)
    {
        if (this.audioSource == null) this.audioSource = this.gameObject.AddComponent<AudioSource>();
        if (!this.dontPlayAudio) Utilities.Audio.playSoundEffect(this.audioSource, audioClip);
        this.dontPlayAudio = true;
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

    public void resetRotation()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual void DestroyIt()
    {
        DestroyIt(0f);
    }

    public virtual void DestroyIt(float delay)
    {
        this.sender.activeSkills.Remove(this);
        //this.isActive = false;
        Destroy(this.gameObject, delay);
    }








    #endregion


    #region Update Extern

    public void updateTimeDistortion(float distortion) //Signal?
    {
        if (this.canAffectedBytimeDistortion)
        {
            this.timeDistortion = 1 + (distortion / 100);

            if (this.animator != null) this.animator.speed = this.timeDistortion;
            if (this.audioSource != null) this.audioSource.pitch = this.timeDistortion;

            SkillProjectile skillProjectile = this.GetComponent<SkillProjectile>();
            if (skillProjectile != null) skillProjectile.updateTimeDistortion(distortion);
        }
    }

    #endregion

}

