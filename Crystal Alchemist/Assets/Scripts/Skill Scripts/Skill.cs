using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

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

    [BoxGroup("Restrictions")]
    [Tooltip("Positions-Offset, damit es nicht im Character anfängt")]
    public float positionOffset = 1f;

    [Space(10)]
    [BoxGroup("Restrictions")]
    [Tooltip("Folgt der Skill dem Charakter")]
    public bool attachToSender = false;

    [BoxGroup("Restrictions")]
    [Tooltip("Während des Skills schaut der Charakter in die gleiche Richtung")]
    public bool lockDirection = false;

    [BoxGroup("Restrictions")]
    [Tooltip("Soll der Skill einer Zeitstörung beeinträchtigt werden?")]
    [SerializeField]
    private bool canAffectedBytimeDistortion = true;

    [BoxGroup("Restrictions")]
    [Tooltip("Hat der Skill eine maximale Dauer?")]
    public bool hasDuration = true;

    [BoxGroup("Restrictions")]
    [ShowIf("hasDuration")]
    [Tooltip("Maximale Dauer des Skills. Zerstört sich danach selbst.")]
    public float maxDuration = 1;


    ////////////////////////////////////////////////////////////////

    [HideInInspector]
    public Character sender;
    [HideInInspector]
    public Character target;
    [HideInInspector]
    public Vector2 direction;
    
    private float durationTimeLeft;
    private float timeDistortion = 1;
    private bool triggerIsActive = true;

    [HideInInspector]
    public bool overridePosition = true;
    #endregion


    #region Start Funktionen (Init, set Basics, Update Sender, set Position

    public void Start()
    {
        setBasicAttributes();
        setPostionAndDirection();
    }

    private void setBasicAttributes()
    {
        if (this.myRigidbody == null) this.myRigidbody = GetComponent<Rigidbody2D>();
        if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.animator == null) this.animator = GetComponent<Animator>();
        if (this.shadow != null && this.spriteRenderer != null) this.shadow.sprite = this.spriteRenderer.sprite;

        this.durationTimeLeft = this.maxDuration;
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
        if (this.spriteRenderer != null && this.shadow != null) this.shadow.sprite = this.spriteRenderer.sprite;

        if (this.animator != null && this.sender != null && !this.lockDirection)
            AnimatorUtil.SetAnimDirection(this.sender.direction, this.animator);
        
        AnimatorUtil.SetAnimatorParameter(this.animator, "Active", true);
        SkillIndicatorModule indicatorModule = this.GetComponent<SkillIndicatorModule>();
        if (indicatorModule != null) indicatorModule.hideIndicator();

        SkillAnimationModule animationModule = this.GetComponent<SkillAnimationModule>();
        if (animationModule != null) animationModule.hideCastingAnimation();

        //Prüfe ob der Skill eine Maximale Dauer hat
        if (this.hasDuration)
        {
            if (this.durationTimeLeft > 0)
            {
                this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);
            }
            else this.DeactivateIt();
        }

        if (this.target != null && !this.target.gameObject.activeInHierarchy) this.DeactivateIt();
    }

    public float getDurationLeft()
    {
        return this.durationTimeLeft;
    }

    public void doOnCast()
    {
        if (this.GetComponent<SkillChainHit>() != null) this.GetComponent<SkillChainHit>().doOnCast();
    }

    public void hitIt(Collider2D hittedObject)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            hitIt(hittedObject.GetComponent<Character>());
        }
    }

    public void hitIt(Character target)
    {
        //Gegner zurückstoßen + Hit
        target.gotHit(this);
    }

    public void hitIt(Collider2D hittedObject, float percentage)
    {
        if (hittedObject.GetComponent<Character>() != null)
        {
            hitIt(hittedObject.GetComponent<Character>(), percentage);
        }
    }

    public void hitIt(Character target, float percentage)
    {
        //Gegner zurückstoßen + Hit
        target.gotHit(this, percentage);
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

        if (blendTreeModule != null)
        {
            blendTree = true;
            useOffSetToBlendTree = blendTreeModule.useOffSetToBlendTree;
        }

        if (positionModule != null)
        {
            //useCustomPosition = positionModule.useGameObjectHeight;
            positionHeight = positionModule.positionHeight;
            colliderHeightOffset = positionModule.colliderHeightOffset;
        }

        if (!blendTree)
        {
            if (!keepOriginalRotation)
            {
                RotationUtil.setDirectionAndRotation(this, out angle, out start, out this.direction, out rotation);
            }

            //if (this.target != null) this.direction = (Vector2)this.target.transform.position - start;                       

            if (this.overridePosition) this.transform.position = start;

            if (keepOriginalRotation)
            {
                this.direction = RotationUtil.DegreeToVector2(this.transform.rotation.eulerAngles.z);
            }

            if (rotateIt && !keepOriginalRotation) transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            float positionX = this.sender.transform.position.x + (this.sender.direction.x * positionOffset);
            float positionY = this.sender.transform.position.y + (this.sender.direction.y * positionOffset) + positionHeight;

            //if (useCustomPosition) positionY = this.sender.skillStartPosition.transform.position.y + (this.sender.direction.y * positionOffset);
            if (useOffSetToBlendTree) this.transform.position = new Vector2(positionX, positionY);
            this.direction = this.sender.direction;
            AnimatorUtil.SetAnimDirection(this.sender.direction, this.animator);
        }

        //AnimatorUtil.SetAnimDirection(this.sender.direction, this.animator);    

        if (this.shadow != null)
        {
            float changeX = 0;
            if (this.direction.y < 0) changeX = this.direction.y;
            this.shadow.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + colliderHeightOffset + (colliderHeightOffset * changeX));
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
        AudioUtil.playSoundEffect(this.gameObject, audioClip);
    }

    public void SetTriggerActive(int value)
    {
        if (value == 0) this.triggerIsActive = false;
        else this.triggerIsActive = true;
    }

    public bool GetTriggerActive()
    {
        return this.triggerIsActive;
    }

    public void resetRotation()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void DeactivateIt()
    {
        AnimatorUtil.SetAnimatorParameter(this.animator, "Explode", true);

        if (this.animator == null || !AnimatorUtil.HasParameter(this.animator, "Explode"))
        {
            //Debug.Log(this.skillName + " hat kein Animator oder Explode-Parameter");
            SetTriggerActive(1);
            DestroyIt();
        }
    }

    public void DestroyIt()
    {
        DestroyIt(0f);
    }

    public void DestroyIt(float delay)
    {  
        this.sender.activeSkills.Remove(this);
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
            if (this.triggerIsActive && this.GetComponent<SkillProjectile>() != null) this.GetComponent<SkillProjectile>().setVelocity();
        }
    }

    public float getTimeDistortion()
    {
        return this.timeDistortion;
    }

    #endregion

}

