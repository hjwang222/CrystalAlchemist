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

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt nur auf sich selbst")]
    public bool affectSelf = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Spieler")]
    public bool affectOther = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectSame = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Gegner")]
    public bool affectNeutral = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("wirkt auf alle Skills")]
    public bool affectSkills = false;

    [BoxGroup("Wirkungsbereich")]
    [Tooltip("Unverwundbarkeit ignorieren (z.B. für Heals)?")]
    public bool ignoreInvincibility = false;
}
