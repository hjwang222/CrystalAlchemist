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
    time,
    intervall,
    hit,
    life,
    mana
}

public enum StatusEffectActionType
{
    resource,
    stacks,
    skill,
    destroy
}



[System.Serializable]
public class StatusEffectTrigger
{
    public StatusEffectTriggerType triggerType;

    [ShowIf("triggerType", StatusEffectTriggerType.time)]
    public float duration;
    [ShowIf("triggerType", StatusEffectTriggerType.time)]
    [Tooltip("Darf der Statuseffekt einen gleichen Effekt überschreiben? (nur bei Typ Time)")]
    public bool canOverride = false;

    [ShowIf("triggerType", StatusEffectTriggerType.intervall)]
    public float intervall;

    [ShowIf("triggerType", StatusEffectTriggerType.life)]
    public float life;

    [ShowIf("triggerType", StatusEffectTriggerType.mana)]
    public float mana;
    [ShowIf("triggerType", StatusEffectTriggerType.mana)]
    [Tooltip("Darf der Statuseffekt den gleichen Effekt deaktivieren? (nur bei Typ Mana)")]
    public bool canDeactivateIt = false;

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

    [ShowIf("actionType", StatusEffectActionType.skill)]
    public List<StandardSkill> skills;
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
    [Tooltip("Anzahl der maximalen gleichen Effekte (Stacks)")]
    [Range(1, Utilities.maxFloatSmall)]
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
    private float elapsedMana;

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

    //Signal?

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion/100);
    }

    public virtual void init()
    {
        if (this.ownAnimator == null) this.ownAnimator = this.GetComponent<Animator>();
        if (this.target != null && this.changeColor) this.target.addColor(this.statusEffectColor);

        doActions();

        /*
        if (this.statusEffectInterval == 0)
        {
            //Einmalig
            doEffect();
            this.statusEffectInterval = Mathf.Infinity;
        }
        else
        {
            //erste Wirkung
            this.elapsed = this.statusEffectInterval;
            this.elapsedMana = this.statusEffectManaDrainInterval;
           } */
        

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    #endregion


    #region Update

    public void Update()
    {
        doOnUpdate();
    }

    private void doActions()
    {
        foreach (StatusEffectEvent buffEvent in this.statusEffectEvents)
        {
            if (isTriggered(buffEvent)) doEvent(buffEvent);
        }
    }

    private bool isTriggered(StatusEffectEvent buffEvent)
    {
        bool isTriggered = false;

        foreach (StatusEffectTrigger trigger in buffEvent.triggers)
        {
            switch (trigger.triggerType)
            {
                case StatusEffectTriggerType.time: if (this.statusEffectTimeLeft >= trigger.duration) isTriggered = true; break;
                case StatusEffectTriggerType.intervall: if (this.elapsed >= trigger.intervall) isTriggered = true; this.elapsed = 0; break;
                case StatusEffectTriggerType.init: isTriggered = true; break;
                case StatusEffectTriggerType.life: if (this.target.life <= trigger.life) isTriggered = true; break;
                case StatusEffectTriggerType.mana: if (this.target.mana <= trigger.mana) isTriggered = true; break;
            }
        }

        return isTriggered;
    }

    private void doEvent(StatusEffectEvent buffEvent)
    {
        foreach (StatusEffectAction action in buffEvent.actions)
        {
            if (action.actionType == StatusEffectActionType.resource)
            {
                foreach (affectedResource resource in action.affectedResources)
                {
                    this.target.updateResource(resource.resourceType, resource.item, resource.amount);
                }
            }
            else if (action.actionType == StatusEffectActionType.stacks)
            {
                Utilities.StatusEffectUtil.RemoveStatusEffect(this, false, this.target);
            }
            else if (action.actionType == StatusEffectActionType.skill)
            {
                foreach (StandardSkill skill in action.skills)
                {
                    Utilities.Skill.instantiateSkill(skill, this.target);
                }
            }
            else if (action.actionType == StatusEffectActionType.destroy)
            {
                this.DestroyIt();
            }
        }
    }

    public virtual void doOnUpdate()
    {
        this.updateUI.Raise();

        this.statusEffectTimeLeft += (Time.deltaTime * this.timeDistortion);
        this.elapsed += (Time.deltaTime * this.timeDistortion);

        doEffect();
        doActions();
    }

   
    /*
    public virtual void doOnUpdate2()
    {
        //TODO: Performance?
        this.updateUI.Raise();

        if (this.endType == StatusEffectEndType.time) this.statusEffectTimeLeft -= (Time.deltaTime * this.timeDistortion);

        this.elapsed += (Time.deltaTime * this.timeDistortion);
        this.elapsedMana += (Time.deltaTime * this.timeDistortion);

        if (this.endType == StatusEffectEndType.mana
            && this.statusEffectManaDrainInterval > 0
            && this.elapsedMana >= statusEffectManaDrainInterval
            && this.target != null
            && this.target.getResource(ResourceType.mana, null) - this.statusEffectManaDrain >= 0)
        {
            //Reduziere Mana solange der Effekt aktiv ist   
            this.target.updateResource(ResourceType.mana, null, -this.statusEffectManaDrain);
            this.elapsedMana = 0;
        }

        if ((this.endType == StatusEffectEndType.time
            && this.statusEffectInterval > 0
            && this.statusEffectTimeLeft > 0
            && this.elapsed >= statusEffectInterval)
            ||
            (this.endType == StatusEffectEndType.mana
            && this.statusEffectInterval > 0
            && this.elapsed >= statusEffectInterval
            && this.target != null
            && this.target.getResource(ResourceType.mana, null) - this.statusEffectManaDrain >= 0))
        {
            //solange der Effekt aktiv ist und das Intervall erreicht ist, soll etwas passieren. Intervall zurück setzen
            doEffect();
            this.elapsed = 0;
        }
        else if ((this.endType == StatusEffectEndType.time
                 && this.statusEffectTimeLeft <= 0)
                 || (this.endType == StatusEffectEndType.mana
            && this.target != null
            && this.target.getResource(ResourceType.mana, null) - this.statusEffectManaDrain < 0))
        {
            //Zerstöre Effekt, wenn die Zeit abgelaufen ist
            DestroyIt();
        }
    }*/

    public virtual void DestroyIt()
    {
        Utilities.UnityUtils.SetAnimatorParameter(this.ownAnimator, "End");

        if (this.target != null)
        {
            //Charakter-Farbe zurücksetzen
            if(this.changeColor) this.target.resetColor(this.statusEffectColor);

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

    public virtual void doEffect()
    {
        //Wirkung abhängig vom Script!
        /*
        if (this.target != null && this.changeColor)
        {
            this.target.addColor(this.statusEffectColor);
        }*/
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        Utilities.Audio.playSoundEffect(this.audioSource, audioClip);
    }



    #endregion
}
