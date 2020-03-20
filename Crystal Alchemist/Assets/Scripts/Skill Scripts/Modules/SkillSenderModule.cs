using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSenderModule : SkillModule
{
    [TabGroup("Sender Attribute")]
    public Price price;

    [TabGroup("Sender Attribute")]
    [HideIf("resourceType", ResourceType.none)]
    [Tooltip("Intervall während der Dauer des Skills Leben oder Mana verändert werden.")]
    [MinValue(0)]
    public float intervallSender = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Bewegungsgeschwindigkeit während eines Casts")]
    [Range(-100, 100)]
    public float speedDuringCasting = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Bewegungsgeschwindigkeit während des Angriffs")]
    [Range(-100, 100)]
    public float speedDuringDuration = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Soll die Geschwindigkeit auch die Animation beeinflussen?")]
    public bool affectAnimation = true;

    [Space(10)]
    [TabGroup("Sender Attribute")]
    [Tooltip("True = nach vorne, False = Knockback")]
    [SerializeField]
    private bool forward = false;

    [TabGroup("Sender Attribute")]
    [MinValue(0)]
    [Tooltip("Stärke des Knockbacks")]
    [SerializeField]
    private float selfThrust = 0;

    [TabGroup("Sender Attribute")]
    [MinValue(0)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("selfThrust", 0f)]
    [SerializeField]
    private float selfThrustTime = 0;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("Soll der Spieler nur diesen Skill benutzen dürfen?")]
    [EnumToggleButtons]
    public StateType stateType = StateType.none;

    private float elapsed;


    private void Start()
    {
        if (this.skill.sender.currentState != CharacterState.dead
        && this.skill.sender.currentState != CharacterState.respawning)
        {
            if (this.stateType == StateType.attack) this.skill.sender.currentState = CharacterState.attack;
            else if (this.stateType == StateType.defend) this.skill.sender.currentState = CharacterState.defend;

            updateResourceSender();
            setSelfTrust();

            this.elapsed = this.intervallSender;
        }
    }

    private void Update()
    {
        if (this.speedDuringDuration != 0) this.skill.sender.updateSpeed(this.speedDuringDuration, this.affectAnimation);

        if (this.intervallSender > 0)
        {
            if (this.elapsed > 0) this.elapsed -= (Time.deltaTime * this.skill.getTimeDistortion());
            else
            {
                if (this.skill.sender != null)
                {
                    if (this.skill.sender.HasEnoughCurrency(this.price)) this.skill.DeactivateIt();
                    else
                    {
                        this.elapsed = this.intervallSender;
                        this.updateResourceSender();
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (this.stateType != StateType.none) this.skill.sender.currentState = CharacterState.idle;
        if (this.speedDuringDuration != 0) this.skill.sender.updateSpeed(0);
    }

    private void updateResourceSender()
    {
        if (this.skill.sender != null) this.skill.sender.updateResource(this.price);
    }

    private void setSelfTrust()
    {
        if (this.selfThrust > 0)
        {
            this.skill.maxDuration = this.selfThrustTime;
            int trustdirection = -1; //knockback
            if (forward) trustdirection = 1; //dash

            this.skill.sender.knockBack(selfThrustTime, selfThrust, (this.skill.sender.direction * trustdirection));
        }
    }
}
