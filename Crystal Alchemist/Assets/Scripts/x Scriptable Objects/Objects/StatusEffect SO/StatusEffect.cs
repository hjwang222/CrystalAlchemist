using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

public enum StatusEffectType
{
    buff,
    debuff    
}

[CreateAssetMenu(menuName = "Game/StatusEffect")]
public class StatusEffect : ScriptableObject
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
    public bool hasDuration = true;

    [FoldoutGroup("Basis Attribute")]
    [ShowIf("hasDuration")]
    [MinValue(1)]
    public float maxDuration = 1;

    [FoldoutGroup("Basis Attribute")]
    public bool canOverride = false;

    [FoldoutGroup("Basis Attribute")]
    public bool canBeModified = true;

    [FoldoutGroup("Basis Attribute")]
    public bool canDeactivateIt = false;

    [FoldoutGroup("Basis Attribute")]
    public bool canBeDispelled = true;

    [FoldoutGroup("Basis Attribute")]
    [Tooltip("Anzahl der maximalen gleichen Effekte (Stacks)")]
    [MinValue(1)]
    public float maxStacks = 1;

    [FoldoutGroup("Basis Attribute")]
    [Tooltip("Ist der Charakter betäubt?")]
    public bool stunTarget = false;

    [FoldoutGroup("Basis Attribute")]
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
    private bool changeColor = true;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Farbe während der Dauer des Statuseffekts")]
    [SerializeField]
    [ShowIf("changeColor")]
    private Color statusEffectColor;

    [FoldoutGroup("Visuals", expanded: false)]
    [SerializeField]
    [Required]
    private StatusEffectGameObject StatusEffectObject;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Icon des Statuseffekts für das UI")]
    [AssetIcon]
    public Sprite iconSprite;

    [FoldoutGroup("Visuals", expanded: false)]
    [Tooltip("Signal zum Update der UI")]
    [SerializeField]
    private SimpleSignal updateUI;

    private Character target;
    private float statusEffectTimeLeft;
    private float timeDistortion = 1;
    private StatusEffectGameObject activeObject;
    #endregion


    #region Start Funktionen (Init)
    public void Initialize(Character character)
    {
        this.target = character;
        if (this.statusEffectType == StatusEffectType.debuff) this.target.debuffs.Add(this);
        else this.target.buffs.Add(this);

        if (this.target != null && this.changeColor) this.target.changeColor(this.statusEffectColor);
        this.activeObject = Instantiate(this.StatusEffectObject, this.target.activeStatusEffectParent.transform.position, Quaternion.identity, this.target.transform);

        setTime();
        initActions();
    }

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion / 100);
    }

    private void setTime()
    {
        this.statusEffectTimeLeft = this.maxDuration;

        if (this.canBeModified)
        {
            float percentage = 0;
            if (this.statusEffectType == StatusEffectType.buff) percentage = (float)this.target.buffPlus;
            else percentage = (float)this.target.debuffMinus;

            this.statusEffectTimeLeft *= ((100f + (float)percentage) / 100f);
        }
    }

    #endregion


    #region Update

    public void Updating()
    {
        doOnUpdate();
    }  

    private void doOnUpdate()
    {
        this.updateUI.Raise();

        if (this.hasDuration && this.statusEffectTimeLeft > 0)
        {
            this.statusEffectTimeLeft -= (Time.deltaTime * this.timeDistortion);
        }

        updateActions();
        if (this.statusEffectTimeLeft <= 0) DestroyIt();
    }

    private void updateActions()
    {
        foreach(StatusEffectEvent effectEvent in this.statusEffectEvents)
        {
            effectEvent.Updating(this.timeDistortion);
            effectEvent.DoEvents(this.target, this);
        }
    }

    private void initActions()
    {
        foreach (StatusEffectEvent effectEvent in this.statusEffectEvents)
        {
            effectEvent.Initialize(this.target, this);
        }
    }

    public void DestroyIt()
    {
        if (this.activeObject != null) this.activeObject.SetEnd();
        if (this.target != null)
        {
            //Charakter-Farbe zurücksetzen
            if (this.changeColor) this.target.removeColor(this.statusEffectColor);

            this.resetValues();
        }

        //GUI updaten und Objekt kurz danach zerstören
        this.updateUI.Raise();
        Destroy(this, this.destroyDelay);
    }

    private void resetValues()
    {
        foreach (StatusEffectEvent buffEvent in this.statusEffectEvents)
        {
            buffEvent.ResetEvent(this.target, this);
        }
    }

    public void changeTime(float extendTimePercentage)
    {
        this.statusEffectTimeLeft += (this.statusEffectTimeLeft * extendTimePercentage) / 100;
    }

    public float getTimeLeft()
    {
        return this.statusEffectTimeLeft;
    }

    public Character getTarget()
    {
        return this.target;
    }

    public void doModule()
    {
        if (this.activeObject.GetComponent<StatusEffectModule>() != null) this.activeObject.GetComponent<StatusEffectModule>().doAction();
    }

    #endregion
}
