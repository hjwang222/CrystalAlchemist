using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public enum TargetingMode
{
    helper,
    manual,
    auto
}

public enum RangeType
{
    none,
    circle,
    view
}

[CreateAssetMenu(menuName = "Game/Ability/Targeting Property")]
public class TargetingProperty : ScriptableObject
{
    [BoxGroup("Zielerfassung")]
    [Tooltip("Manual = Spieler kann Ziele auswählen, Single = näheste Ziel, Multi = Alle in Range, Auto = Sofort ohne Bestätigung")]
    public TargetingMode targetingMode = TargetingMode.manual;
    
    [BoxGroup("Zielerfassung")]
    [HideIf("targetingMode", TargetingMode.helper)]
    public int maxAmountOfTargets = 1;

    [BoxGroup("Zielerfassung")]
    public bool hasMaxDuration = true;

    [ShowIf("hasMaxDuration")]
    [BoxGroup("Zielerfassung")]
    public float maxDuration = 20f;

    [BoxGroup("Zielerfassung")]
    [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
    [HideIf("targetingMode", TargetingMode.helper)]
    [Range(0, 10)]
    public float multiHitDelay = 0;

    [BoxGroup("Zielerfassung")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    [HideIf("targetingMode", TargetingMode.helper)]
    public bool showRange = false;

    [BoxGroup("Zielerfassung")]
    [HideIf("targetingMode", TargetingMode.helper)]
    public RangeType rangeType = RangeType.circle;

    [BoxGroup("Zielerfassung")]
    [HideIf("targetingMode", TargetingMode.helper)]
    [HideIf("rangeType", RangeType.none)]
    public float range = 6f;

    [BoxGroup("Indikator")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    [HideLabel]
    [SerializeField]
    private IndicatorObject indicator;


    public void UpdateIndicator(Character sender, List<Character> selectedTargets)
    {
        this.indicator.UpdateIndicator(sender, selectedTargets, this.targetingMode);
    }

    public void ClearIndicator()
    {
        this.indicator.ClearIndicator();
    }
}
