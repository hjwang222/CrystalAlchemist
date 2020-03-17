using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTargetModule : SkillModule
{    
    [TabGroup("Ziel Attribute")]
    [Tooltip("Veränderung des Ziels. Negativ = Schaden, Positiv = Heilung")]
    public List<Price> affectedResources;

    [TabGroup("Ziel Attribute")]
    [Tooltip("Statuseffekte")]
    public List<StatusEffect> statusEffects;

    [Space(10)]
    [TabGroup("Ziel Attribute")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    [Tooltip("Stärke des Knockbacks")]
    public float thrust = 4;

    [TabGroup("Ziel Attribute")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    [Tooltip("Dauer des Knockbacks")]
    [HideIf("thrust", 0f)]
    public float knockbackTime = 0.2f;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt nur auf sich selbst")]
    public bool affectSelf = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectOther = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectSame = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectNeutral = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("wirkt auf alle Skills")]
    public bool affectSkills = false;

    [FoldoutGroup("Wirkungsbereich", expanded: false)]
    [Tooltip("Unverwundbarkeit ignorieren (z.B. für Heals)?")]
    public bool ignoreInvincibility = false;
}
