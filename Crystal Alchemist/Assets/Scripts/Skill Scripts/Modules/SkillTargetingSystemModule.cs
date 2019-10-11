using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTargetingSystemModule : SkillModule
{
    [FoldoutGroup("Zielerfassung", expanded: false)]
    public bool setTargetAutomatically = false;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [Tooltip("Ob das Ziel erfasst werden soll. Wenn NULL, dann nicht.")]
    public GameObject lockOn;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Tooltip("Manual = Spieler kann Ziele auswählen, Single = näheste Ziel, Multi = Alle in Range, Auto = Sofort ohne Bestätigung")]
    [EnumToggleButtons]
    public TargetingMode targetingMode = TargetingMode.manual;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Range(0, Utilities.maxIntSmall)]
    public int maxAmountOfTargets = 1;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Range(0, Utilities.maxFloatInfinite)]
    public float targetingDuration = 6f;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
    [Range(0, 10)]
    public float multiHitDelay = 0;

    [FoldoutGroup("Zielerfassung", expanded: false)]
    [ShowIf("lockOn")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showRange = false;
}
