using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTargetModule : SkillModule
{    
    [BoxGroup("Ziel Attribute")]
    [Tooltip("Veränderung des Ziels. Negativ = Schaden, Positiv = Heilung")]
    public List<CharacterResource> affectedResources;

    [BoxGroup("Ziel Attribute")]
    [Tooltip("Statuseffekte")]
    public List<StatusEffect> statusEffects;

    [Space(10)]
    [BoxGroup("Ziel Attribute")]
    [MinValue(0)]
    [Tooltip("Stärke des Knockbacks")]
    public float thrust = 2;

    [BoxGroup("Ziel Attribute")]
    [MinValue(0)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("thrust", 0f)]
    public float knockbackTime = 0.2f;

    [BoxGroup("Ziel Attribute")]
    [Required]
    public SkillAffections affections;
}
