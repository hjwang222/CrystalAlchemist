using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSenderModule : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    [TabGroup("Sender Attribute")]
    [EnumToggleButtons]
    [Tooltip("Art der Resource")]
    public ResourceType resourceType = ResourceType.mana;

    [TabGroup("Sender Attribute")]
    [ShowIf("resourceType", ResourceType.item)]
    [Tooltip("Benötigtes Item")]
    public Item item;

    [TabGroup("Sender Attribute")]
    [HideIf("resourceType", ResourceType.none)]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    [Tooltip("Höhe der Resource des Senders. Negativ = Schaden, Positiv = Heilung")]
    public float addResourceSender = 0;

    [TabGroup("Sender Attribute")]
    [HideIf("resourceType", ResourceType.none)]
    [Range(0, Utilities.maxFloatInfinite)]
    [Tooltip("Intervall während der Dauer des Skills Leben oder Mana verändert werden.")]
    public float intervallSender = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Bewegungsgeschwindigkeit während eines Casts")]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float speedDuringCasting = 0;

    [TabGroup("Sender Attribute")]
    [Tooltip("Bewegungsgeschwindigkeit während des Angriffs")]
    [Range(-Utilities.maxFloatSmall, Utilities.maxFloatSmall)]
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
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Stärke des Knockbacks")]
    [SerializeField]
    private float selfThrust = 0;

    [TabGroup("Sender Attribute")]
    [Range(0, Utilities.maxFloatSmall)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("selfThrust", 0f)]
    [SerializeField]
    private float selfThrustTime = 0;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("Soll der Spieler nur diesen Skill benutzen dürfen?")]
    [EnumToggleButtons]
    public StateType stateType = StateType.none;




    private void Start()
    {
        updateResourceSender();
        setSelfTrust();

        this.skill.elapsed = this.intervallSender;

        if (this.stateType == StateType.attack) this.skill.sender.currentState = CharacterState.attack;
        else if (this.stateType == StateType.defend) this.skill.sender.currentState = CharacterState.defend;
    }

    private void Update()
    {
        if (this.speedDuringDuration != 0) this.skill.sender.updateSpeed(this.speedDuringDuration, this.affectAnimation);

        if (this.intervallSender > 0)
        {
            if (this.skill.elapsed > 0) this.skill.elapsed -= (Time.deltaTime * this.skill.timeDistortion);
            else
            {
                if (this.skill.sender != null)
                {
                    if (this.skill.sender.getResource(this.resourceType, this.item) + this.addResourceSender < 0)
                        this.skill.durationTimeLeft = 0;
                    else
                    {
                        this.skill.elapsed = this.intervallSender;
                        this.skill.sender.updateResource(this.resourceType, this.item, this.addResourceSender);
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
        if (this.skill.sender != null)
        {
            this.skill.sender.updateResource(this.resourceType, this.item, this.addResourceSender);
            //setPostionAndDirection();
        }
    }







    private void setSelfTrust()
    {
        if (this.selfThrust > 0)
        {
            this.skill.duration = this.selfThrustTime;
            int trustdirection = -1; //knockback
            if (forward) trustdirection = 1; //dash

            this.skill.sender.knockBack(selfThrustTime, selfThrust, (this.skill.sender.direction * trustdirection));
        }
    }
}
