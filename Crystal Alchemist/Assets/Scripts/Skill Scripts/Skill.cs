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

    [Space(10)]
    [BoxGroup("Easy Access")]
    public SpriteRenderer spriteRenderer;

    [BoxGroup("Easy Access")]
    public Rigidbody2D myRigidbody;

    [BoxGroup("Easy Access")]
    public Animator animator;

    [BoxGroup("Easy Access")]
    [Tooltip("Schatten")]
    public SpriteRenderer shadow;




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

    private float positionOffset;
    private bool lockDirection;
    private bool canAffectedBytimeDistortion;
    private bool hasDuration;
    private float maxDuration;
    private bool attached;
    #endregion


    #region Start Funktionen (Init, set Basics, Update Sender, set Position

    public void Initialize(float offset, bool lockDirection, bool affectTimeDistortion, bool attached)
    {
        this.positionOffset = offset;
        this.lockDirection = lockDirection;
        this.canAffectedBytimeDistortion = affectTimeDistortion;
        this.attached = attached;
    }

    public void SetMaxDuration(bool hasDuration, float maxDuration)
    {
        this.hasDuration = hasDuration;
        this.maxDuration = maxDuration;
    }

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


    #endregion


    #region Update Funktionen   

    public void Update()
    {
        if (this.spriteRenderer != null && this.shadow != null) this.shadow.sprite = this.spriteRenderer.sprite;

        if (this.animator != null && !this.lockDirection)
            AnimatorUtil.SetAnimDirection(this.direction, this.animator);

        AnimatorUtil.SetAnimatorParameter(this.animator, "Active", true);

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

    public float getMaxDuration()
    {
        return this.maxDuration;
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
            positionHeight = positionModule.positionHeight;
            colliderHeightOffset = positionModule.colliderHeightOffset;
        }

        this.direction = RotationUtil.DegreeToVector2(this.transform.eulerAngles.z);

        if (!blendTree)
        {
            if (this.overridePosition) this.transform.position = start;

            if (!keepOriginalRotation) RotationUtil.setDirectionAndRotation(this, out start, out this.direction, out rotation);
            else this.direction = RotationUtil.DegreeToVector2(this.transform.rotation.eulerAngles.z);

            if (rotateIt && !keepOriginalRotation) transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            if(this.sender != null) this.direction = this.sender.direction.normalized;

            float positionX = this.sender.transform.position.x + (this.direction.x * positionOffset);
            float positionY = this.sender.transform.position.y + (this.direction.y * positionOffset) + positionHeight;

            if (useOffSetToBlendTree) this.transform.position = new Vector2(positionX, positionY);

            AnimatorUtil.SetAnimDirection(this.direction, this.animator);
        }

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

    public bool isAttachedToSender()
    {
        return this.attached;
    }

    public bool isDirectionLocked()
    {
        return this.lockDirection;
    }

    public float GetOffset()
    {
        return this.positionOffset;
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
        if (this.animator == null || !AnimatorUtil.HasParameter(this.animator, "Explode"))
        {
            SetTriggerActive(1);
            DestroyIt();
        }
        else AnimatorUtil.SetAnimatorParameter(this.animator, "Explode", true);
    }

    public void DestroyIt()
    {
        DestroyIt(0f);
    }

    public void DestroyIt(float delay)
    {
        if(this.sender != null) this.sender.activeSkills.Remove(this);
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

