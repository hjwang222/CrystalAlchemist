using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

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

    [BoxGroup("Actions")]
    [SerializeField]
    public UnityEvent OnStart;

    [BoxGroup("Actions")]
    [SerializeField]
    public UnityEvent AfterDelay;


    [BoxGroup("Debug")]
    public Character sender;
    [BoxGroup("Debug")]
    public Character target;
    [BoxGroup("Debug")]
    public Vector2 direction = Vector2.right;
    [BoxGroup("Debug")]
    public bool standAlone = true;

    ////////////////////////////////////////////////////////////////

    private float durationTimeLeft;
    private float delayTimeLeft;

    private float timeDistortion = 1;
    private bool triggerIsActive = true;
    private float positionOffset;
    private bool lockDirection;
    private bool canAffectedBytimeDistortion;

    private bool hasDelay;
    private float delay;
    private bool hasDuration;
    private float maxDuration;

    private bool attached;
    private float progress;

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

    public void SetDelay(bool hasDelay, float delay)
    {
        this.hasDelay = hasDelay;
        this.delay = delay;
    }

    public void SetStandAlone(bool value) => this.standAlone = value;

    private void Start()
    {
        SetComponents();

        if (!this.standAlone)
        {
            this.transform.position = this.sender.GetShootingPosition();
            this.direction = RotationUtil.SetStartDirection(this);

            if (this.GetComponent<SkillRotationModule>() != null) this.GetComponent<SkillRotationModule>().Initialize();
            if (this.GetComponent<SkillPositionZModule>() != null) this.GetComponent<SkillPositionZModule>().Initialize();
            if (this.GetComponent<SkillBlendTreeModule>() != null) this.GetComponent<SkillBlendTreeModule>().Initialize();
        }
        else
        {
            if (this.GetComponent<SkillRotationModule>() != null) this.GetComponent<SkillRotationModule>().Initialize();
            this.direction = RotationUtil.DegreeToVector2(this.transform.rotation.eulerAngles.z);            
        }

        SkillModule[] modules = this.GetComponents<SkillModule>();
        for (int i = 0; i < modules.Length; i++) modules[i].Initialize();

        SkillExtension[] extensions = this.GetComponents<SkillExtension>();
        for (int i = 0; i < extensions.Length; i++) extensions[i].Initialize();

        SkillHitTrigger[] trigger = this.GetComponents<SkillHitTrigger>();
        for (int i = 0; i < trigger.Length; i++) trigger[i].Initialize();

        this.OnStart?.Invoke();
    }

    private void SetComponents()
    {
        if (this.myRigidbody == null) this.myRigidbody = GetComponent<Rigidbody2D>();
        if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.animator == null) this.animator = GetComponent<Animator>();
        if (this.shadow != null && this.spriteRenderer != null) this.shadow.sprite = this.spriteRenderer.sprite;

        this.durationTimeLeft = this.maxDuration;
        this.delayTimeLeft = this.delay;
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

        if (this.hasDelay)
        {
            if (this.delayTimeLeft > 0)
            {
                this.delayTimeLeft -= (Time.deltaTime * this.timeDistortion);
                this.progress = 1 - (this.delayTimeLeft / this.delay);
            }
            else
            {
                this.AfterDelay?.Invoke();
                this.hasDelay = false;
            }
        }

        //Prüfe ob der Skill eine Maximale Dauer hat
        if (this.hasDuration && !this.hasDelay)
        {
            if (this.durationTimeLeft > 0) this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);            
            else this.DeactivateIt();
        }

        if (this.target != null && !this.target.gameObject.activeInHierarchy) this.DeactivateIt();

        SkillModule[] modules = this.GetComponents<SkillModule>();
        for (int i = 0; i < modules.Length; i++) modules[i].Updating();

        SkillExtension[] extensions = this.GetComponents<SkillExtension>();
        for (int i = 0; i < extensions.Length; i++) extensions[i].Updating();

        SkillHitTrigger[] trigger = this.GetComponents<SkillHitTrigger>();
        for (int i = 0; i < trigger.Length; i++) trigger[i].Updating();
    }

    public float GetDurationLeft()
    {
        return this.durationTimeLeft;
    }

    public float GetMaxDuration()
    {
        return this.maxDuration;
    }

    public float GetOffset()
    {
        return this.positionOffset;
    }

    public float GetProgress()
    {
        return this.progress;
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

    public bool isAttachedToSender()
    {
        return this.attached;
    }

    public bool isDirectionLocked()
    {
        return this.lockDirection;
    }

    #region AnimatorEvents

    public void PlaySoundEffect(AudioClip audioClip)
    {
        AudioUtil.playSoundEffect(this.gameObject, audioClip);
    }

    public void PlayAnimation(string trigger)
    {
        AnimatorUtil.SetAnimatorParameter(this.animator, trigger);
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
        if(this.sender != null) this.sender.values.activeSkills.Remove(this);
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

