using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

#region Enums
public enum StatusEffectType
{
    buff,
    debuff
}

public enum StatusEffectTriggerType
{
    init,
    intervall,
    hit,
    life,
    mana,
    destroyed
}

public enum StatusEffectActionType
{
    resource,
    stacks,
    skill,
    destroy,
    speed,
    time,
    module,
    immortal
}

[System.Serializable]
public class StatusEffectTrigger
{
    public StatusEffectTriggerType triggerType;

    [ShowIf("triggerType", StatusEffectTriggerType.intervall)]
    public float intervall;

    [ShowIf("triggerType", StatusEffectTriggerType.life)]
    public float life;

    [ShowIf("triggerType", StatusEffectTriggerType.mana)]
    public float mana;

    [ShowIf("triggerType", StatusEffectTriggerType.hit)]
    public float hits;

    public bool requireAll = false;
}

[System.Serializable]
public class StatusEffectAction
{
    public StatusEffectActionType actionType;

    [ShowIf("actionType", StatusEffectActionType.resource)]
    [SerializeField]
    public List<affectedResource> affectedResources;

    [ShowIf("actionType", StatusEffectActionType.stacks)]
    public int amount;

    [ShowIf("actionType", StatusEffectActionType.speed)]
    [Range(-100, 100)]
    public float speed;

    [ShowIf("actionType", StatusEffectActionType.time)]
    [Range(-100, 100)]
    public float time;

    [ShowIf("actionType", StatusEffectActionType.skill)]
    public List<Skill> skills;    

    [ShowIf("actionType", StatusEffectActionType.module)]
    public StatusEffectModule module;
}

[System.Serializable]
public class StatusEffectEvent
{
    public List<StatusEffectTrigger> triggers;
    public List<StatusEffectAction> actions;
}

#endregion

public class StatusEffect : MonoBehaviour
{
    #region Attribute
    [BoxGroup("Statuseffekt Pflichtfelder")]
    [Required]
    [Tooltip("Name des Statuseffekts")]
    public string statusEffectName;

    [BoxGroup("Statuseffekt Pflichtfelder")]
    [Required]
    [Tooltip("Name des Statuseffekts")]
    public string statusEffectNameEnglish;

    [FoldoutGroup("RPG Elemente")]
    [TextArea]
    [Tooltip("Beschreibung des Statuseffekts")]
    public string statusEffectDescription;

    [FoldoutGroup("RPG Elemente")]
    [TextArea]
    [Tooltip("Beschreibung des Statuseffekts")]
    public string statusEffectDescriptionEnglish;       

    [FoldoutGroup("Basis Attribute")]
    [Range(0, CustomUtilities.maxFloatInfinite)]
    public float maxDuration = 0;

    [FoldoutGroup("Basis Attribute")]
    public bool canOverride = false;

    [FoldoutGroup("Basis Attribute")]
    public bool canBeChanged = true;

    [FoldoutGroup("Basis Attribute")]
    public bool canDeactivateIt = false;

    [FoldoutGroup("Basis Attribute")]
    public bool canBeDispelled = true;

    [FoldoutGroup("Basis Attribute")]
    [Tooltip("Anzahl der maximalen gleichen Effekte (Stacks)")]
    [Range(1, CustomUtilities.maxFloatSmall)]
    public float maxStacks = 1;

    [FoldoutGroup("Basis Attribute")]
    [Tooltip("Ist der Charakter betäubt?")]
    public bool stunTarget = false;

    [FoldoutGroup("Basis Attribute")]
    [EnumToggleButtons]
    [Tooltip("Handelt es sich um einen positiven oder negativen Effekt?")]
    public StatusEffectType statusEffectType = StatusEffectType.debuff;

    [FoldoutGroup("Trigger and Actions", expanded: false)]
    public List<StatusEffectEvent> statusEffectEvents = new List<StatusEffectEvent>();

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Farbe während der Dauer des Statuseffekts")]
    [SerializeField]
    private float destroyDelay = 0.2f;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Farbe während der Dauer des Statuseffekts")]
    [SerializeField]
    private Animator ownAnimator;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Farbe während der Dauer des Statuseffekts")]
    [SerializeField]
    private bool changeColor = true;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Farbe während der Dauer des Statuseffekts")]
    [SerializeField]
    [ShowIf("changeColor")]
    private Color statusEffectColor;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Icon des Statuseffekts für das UI")]
    public Sprite iconSprite;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Signal zum Update der UI")]
    [SerializeField]
    private SimpleSignal updateUI;

    private AudioSource audioSource;
    private float elapsed;
    [HideInInspector]
    public Character target;
    [HideInInspector]
    public float statusEffectTimeLeft;
    [HideInInspector]
    public float timeDistortion = 1;
    #endregion


    #region Start Funktionen (Init)
    public void Start()
    {
        init();        
    }

    public void setTarget(Character character)
    {
        this.target = character;

        this.gameObject.transform.SetParent(character.activeStatusEffectParent.transform, false);
        //DontDestroyOnLoad(this.gameObject);

        if (this.statusEffectType == StatusEffectType.debuff) character.debuffs.Add(this);
        else character.buffs.Add(this);
    }

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion/100);
    }

    private void init()
    {
        if (this.ownAnimator == null) this.ownAnimator = this.GetComponent<Animator>();
        if (this.target != null && this.changeColor) this.target.changeColor(this.statusEffectColor);

        setTime();
        doActions(true);

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        //this.updateUI.Raise();
    }

    private void setTime()
    {
        this.statusEffectTimeLeft = this.maxDuration;

        if (this.canBeChanged)
        {
            float percentage = 0;
            if (this.statusEffectType == StatusEffectType.buff) percentage = (float)this.target.buffPlus;
            else percentage = (float)this.target.debuffMinus;

            this.statusEffectTimeLeft *= ((100f + (float)percentage) / 100f);
        }        
    }

    #endregion


    #region Update

    public void Update()
    {
        doOnUpdate();
    }

    private void doActions(bool isInit)
    {
        foreach (StatusEffectEvent buffEvent in this.statusEffectEvents)
        {
            if (isTriggered(buffEvent, isInit)) doEvent(buffEvent);
        }
    }

    private bool isTriggered(StatusEffectEvent buffEvent, bool isInit)
    {
        bool isTriggered = false;

        foreach (StatusEffectTrigger trigger in buffEvent.triggers)
        {
            switch (trigger.triggerType)
            {                
                case StatusEffectTriggerType.intervall: if (this.elapsed >= trigger.intervall) { isTriggered = true; this.elapsed = 0; } break;
                case StatusEffectTriggerType.init: if (isInit) isTriggered = true; break;
                case StatusEffectTriggerType.life: if (this.target != null && this.target.life <= trigger.life) isTriggered = true; break;
                case StatusEffectTriggerType.mana: if (this.target != null && this.target.mana <= trigger.mana) isTriggered = true; break;
                case StatusEffectTriggerType.destroyed: if (this.statusEffectTimeLeft <= 0) isTriggered = true; break;
            }
        }

        return isTriggered;
    }

    private void doEvent(StatusEffectEvent buffEvent)
    {
        foreach (StatusEffectAction action in buffEvent.actions)
        {
            if (action.actionType == StatusEffectActionType.resource && this.target != null)
            {
                foreach (affectedResource resource in action.affectedResources)
                {
                    this.target.updateResource(resource.resourceType, resource.item, resource.amount);
                }
            }
            else if (action.actionType == StatusEffectActionType.speed)
            {
                this.target.updateSpeed(action.speed);
            }
            else if (action.actionType == StatusEffectActionType.time)
            {
                target.updateTimeDistortion(action.time);
            }
            else if (action.actionType == StatusEffectActionType.module)
            {
                action.module.doAction();
            }
            else if (action.actionType == StatusEffectActionType.stacks)
            {
                CustomUtilities.StatusEffectUtil.RemoveStatusEffect(this, false, this.target);
            }
            else if (action.actionType == StatusEffectActionType.skill)
            {
                foreach (Skill skill in action.skills)
                {
                    CustomUtilities.Skills.instantiateSkill(skill, this.target);
                }
            }
            else if (action.actionType == StatusEffectActionType.immortal)
            {
                if(this.target != null) this.target.cannotDie = true;
            }
            else if (action.actionType == StatusEffectActionType.destroy)
            {
                this.DestroyIt();
            }
        }
    }

    
    private void doOnUpdate()
    {
        this.updateUI.Raise();

        if (this.maxDuration < CustomUtilities.maxFloatInfinite)
        {
            this.statusEffectTimeLeft -= (Time.deltaTime * this.timeDistortion);            
        }

        this.elapsed += (Time.deltaTime * this.timeDistortion);

        doActions(false);

        if (this.statusEffectTimeLeft <= 0) DestroyIt();
    }   
 
    public void DestroyIt()
    {
        CustomUtilities.UnityUtils.SetAnimatorParameter(this.ownAnimator, "End");
               
        if (this.target != null)
        {
            //Charakter-Farbe zurücksetzen
            if(this.changeColor) this.target.removeColor(this.statusEffectColor);

            this.resetValues();

            //Statuseffekt von der Liste entfernen
            if (this.statusEffectType == StatusEffectType.debuff)
            {
                this.target.GetComponent<Character>().debuffs.Remove(this);
            }
            else if (this.statusEffectType == StatusEffectType.buff)
            {
                this.target.GetComponent<Character>().buffs.Remove(this);
            }            
        }

        //GUI updaten und Objekt kurz danach zerstören
        this.updateUI.Raise();
        Destroy(this.gameObject, this.destroyDelay);
    }

    private void resetValues()
    {
        foreach (StatusEffectEvent buffEvent in this.statusEffectEvents)
        {
            foreach (StatusEffectAction action in buffEvent.actions)
            {
                if (action.actionType == StatusEffectActionType.speed)
                {
                    this.target.updateSpeed(0);
                }
                else if (action.actionType == StatusEffectActionType.time)
                {
                    target.updateTimeDistortion(0);
                }                  
                else if (action.actionType == StatusEffectActionType.immortal)
                {
                    if (this.target != null) this.target.cannotDie = false;
                }                
            }
        }
    }

    
    public void PlaySoundEffect(AudioClip audioClip)
    {
        CustomUtilities.Audio.playSoundEffect(this.gameObject, audioClip);
    }



    #endregion
}
