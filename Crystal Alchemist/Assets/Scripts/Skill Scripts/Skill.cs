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

public class Skill : MonoBehaviour
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
    [FoldoutGroup("Easy Access", expanded: false)]
    public SpriteRenderer spriteRenderer;

    [FoldoutGroup("Easy Access", expanded: false)]
    public Rigidbody2D myRigidbody;

    [FoldoutGroup("Easy Access", expanded: false)]
    public Animator animator;

    [FoldoutGroup("Easy Access", expanded: false)]
    [Tooltip("Schatten")]
    public SpriteRenderer shadow;


    ////////////////////////////////////////////////////////////////

    [FoldoutGroup("Behavior", expanded: false)]
    [Tooltip("Geschwindigkeit des Projektils")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    public float speed = 0;

    [FoldoutGroup("Behavior", expanded: false)]
    [Range(0, 2)]
    [Tooltip("Positions-Offset, damit es nicht im Character anfängt")]
    public float positionOffset = 1f;

    [FoldoutGroup("Behavior", expanded: false)]
    [Range(1, CustomUtilities.maxIntInfinite)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe")]
    public int maxAmounts = CustomUtilities.maxIntInfinite;

    [Space(10)]
    [FoldoutGroup("Behavior", expanded: false)]
    [Tooltip("Folgt der Skill dem Sender")]
    public bool attachToSender = false;

    [FoldoutGroup("Behavior", expanded: false)]
    [Tooltip("Soll etwas während des Delays getan werden (DoCast Methode)")]
    public bool doCastDuringDelay = false;

    [FoldoutGroup("Behavior", expanded: false)]
    [Tooltip("Maximale Anzahl aktiver gleicher Angriffe in einer Combo")]
    public int comboAmount = CustomUtilities.maxIntSmall;


    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Castzeit bis zur Instanziierung (für Außen)")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    public float cast = 0;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Soll der Charakter während des Schießens weiterhin in die gleiche Richtung schauen?")]
    [SerializeField]
    [Range(0, CustomUtilities.maxFloatInfinite)]
    private float lockMovementonDuration = 0;

    [Space(10)]
    [FoldoutGroup("Controls", expanded: false)]
    public bool deactivateByButtonUp = false;

    [FoldoutGroup("Controls", expanded: false)]
    public bool deactivateByButtonDown = false;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Kann der Knopf gedrückt gehalten werden für weitere Schüsse?")]
    public bool isRapidFire = false;

    [FoldoutGroup("Controls", expanded: false)]
    [Tooltip("Soll die Castzeit gespeichert bleiben. True = ja. Ansonsten reset bei Abbruch ")]
    public bool keepHoldTimer = false;

    [FoldoutGroup("Controls", expanded: false)]
    [SerializeField]
    private bool canAffectedBytimeDistortion = true;

    
    [FoldoutGroup("Time", expanded: false)]
    [Tooltip("Verzögerung bis Aktivierung")]
    [Range(0, CustomUtilities.maxFloatInfinite)]
    public float delay = 0;

    [FoldoutGroup("Time", expanded: false)]
    [Tooltip("Dauer des Angriffs. 0 = Animation, max bis Trigger")]
    [Range(0, CustomUtilities.maxFloatInfinite)]
    public float duration = 1;

    [FoldoutGroup("Time", expanded: false)]
    [Tooltip("Abklingzeit nach Aktivierung (für Außen)")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    public float cooldown = 1;

    [Space(10)]
    [FoldoutGroup("Time", expanded: false)]
    [Tooltip("Abklingzeit nach Kombo")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    public float cooldownAfterCombo = 0;

    [FoldoutGroup("Time", expanded: false)]
    [Tooltip("Zeit für eine Kombo")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    public float durationCombo = 0;






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
    public bool overridePosition = true;
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
        setBasicAttributes();
        setPostionAndDirection();

        if (this.sender == null)
        {
            throw new System.Exception("No SENDER found! Must be player!");
        }
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
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", this.sender.direction.x);
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", this.sender.direction.y);
        }

        if (this.delayTimeLeft > 0)
        {
            SkillIndicatorModule indicatorModule = this.GetComponent<SkillIndicatorModule>();
            if (indicatorModule != null) indicatorModule.showIndicator();

            this.delayTimeLeft -= (Time.deltaTime * this.timeDistortion);

            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "Time", this.delayTimeLeft);

            if (this.doCastDuringDelay) doOnCast();
        }
        else
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "Active", true);
            SkillIndicatorModule indicatorModule = this.GetComponent<SkillIndicatorModule>();
            if (indicatorModule != null) indicatorModule.hideIndicator();

            SkillAnimationModule animationModule = this.GetComponent<SkillAnimationModule>();
            if (animationModule != null) animationModule.hideCastingAnimation();

            //Prüfe ob der Skill eine Maximale Dauer hat
            if (this.durationTimeLeft < CustomUtilities.maxFloatInfinite)
            {
                if (this.durationTimeLeft > 0)
                {
                    this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);
                }
                else
                {
                    CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "Explode", true);

                    if (this.animator == null || !CustomUtilities.UnityUtils.HasParameter(this.animator, "Explode"))
                    {
                        //Debug.Log(this.skillName + " hat kein Animator oder Explode-Parameter");
                        SetTriggerActive(1);
                        DestroyIt();
                    }
                }
            }
        }

        if (this.target != null && !this.target.gameObject.activeInHierarchy) this.durationTimeLeft = 0;

    }

    public float getDurationLeft()
    {
        return this.durationTimeLeft;
    }

    public void doOnCast()
    {
        if (this.GetComponent<SkillChain>() != null) this.GetComponent<SkillChain>().doOnCast();
    }    

    public void hitIt(Collider2D hittedObject)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            //Gegner zurückstoßen + Hit
            hittedObject.GetComponent<Character>().gotHit(this);
        }
    }

    public void hitIt(Collider2D hittedObject, float percentage)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            //Gegner zurückstoßen + Hit
            hittedObject.GetComponent<Character>().gotHit(this, percentage);
        }
    }

    #endregion


    public void setPostionAndDirection()
    {
        //Bestimme Winkel und Position

        float angle = 0;
        Vector2 start = this.transform.position;
        Vector3 rotation = this.transform.rotation.eulerAngles;

        bool blendTree = false;
        bool useOffSetToBlendTree = false;
        bool keepOriginalRotation = false;
        bool rotateIt = false;

        float positionOffset = this.positionOffset;
        float positionHeight = 0;
        float colliderHeightOffset = 0;

        SkillRotationModule rotationModule = this.GetComponent<SkillRotationModule>();
        SkillBlendTreeModule blendTreeModule = this.GetComponent<SkillBlendTreeModule>();
        SkillPositionZModule positionModule = this.GetComponent<SkillPositionZModule>();

        if (rotationModule != null)
        {
            keepOriginalRotation = rotationModule.keepOriginalRotation;            
            rotateIt = rotationModule.rotateIt;            
        }

        if(blendTreeModule != null)
        {
            blendTree = true;
            useOffSetToBlendTree = blendTreeModule.useOffSetToBlendTree;
        }

        if(positionModule != null)
        {
            //useCustomPosition = positionModule.useGameObjectHeight;
            positionHeight = positionModule.positionHeight;            
            colliderHeightOffset = positionModule.colliderHeightOffset;
        }

        if (!blendTree)
        {
            if (!keepOriginalRotation)
            {
                CustomUtilities.Rotation.setDirectionAndRotation(this, out angle, out start, out this.direction, out rotation);
            }

            //if (this.target != null) this.direction = (Vector2)this.target.transform.position - start;                       

            if (this.overridePosition) this.transform.position = start;

            if (keepOriginalRotation)
            {
                this.direction = CustomUtilities.Rotation.DegreeToVector2(this.transform.rotation.eulerAngles.z);
            }

            if (rotateIt && !keepOriginalRotation) transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            float positionX = this.sender.transform.position.x + (this.sender.direction.x * positionOffset);
            float positionY = this.sender.transform.position.y + (this.sender.direction.y * positionOffset) + positionHeight;

            //if (useCustomPosition) positionY = this.sender.skillStartPosition.transform.position.y + (this.sender.direction.y * positionOffset);
            if (useOffSetToBlendTree) this.transform.position = new Vector2(positionX, positionY);
        }

        if (this.animator != null)
        {
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "moveX", this.sender.direction.x);
            CustomUtilities.UnityUtils.SetAnimatorParameter(this.animator, "moveY", this.sender.direction.y);
        }

        if (this.shadow != null)
        {
            float changeX = 0;
            if (this.direction.y < 0) changeX = this.direction.y;
            this.shadow.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + colliderHeightOffset + (colliderHeightOffset * changeX));
        }
    }
       
    public bool isResourceEnough(Character character)
    {
        SkillSenderModule senderModule = this.GetComponent<SkillSenderModule>();

        if (senderModule == null) return true;
        else
        {
            if (character.getResource(senderModule.resourceType, senderModule.item) + senderModule.addResourceSender >= 0 //new method: Check if enough resource on skill
                           || senderModule.addResourceSender == -CustomUtilities.maxFloatInfinite) return true;
            else return false;
        }
    }

    public bool rotateIt()
    {
        if (this.GetComponent<SkillRotationModule>() != null) return this.GetComponent<SkillRotationModule>().rotateIt;
        else return false;
    }


    #region AnimatorEvents

    public void PlaySoundEffect(AudioClip audioClip)
    {
        CustomUtilities.Audio.playSoundEffect(this.gameObject, audioClip);
    }

    public void PlaySoundEffectOnce(AudioClip audioClip)
    {
        if (this.audioSource == null) this.audioSource = this.gameObject.AddComponent<AudioSource>();
        if (!this.dontPlayAudio) CustomUtilities.Audio.playSoundEffect(this.gameObject, audioClip);
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

    public void DestroyIt()
    {
        DestroyIt(0f);
    }

    public void DestroyIt(float delay)
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
            if (this.myRigidbody != null && this.isActive) this.myRigidbody.velocity = this.direction.normalized * this.speed * this.timeDistortion;
        }
    }

    #endregion

}

