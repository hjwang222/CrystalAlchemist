using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public enum TargetingMode
{
    manual,
    auto
}

public enum RangeType
{
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
    public int maxAmountOfTargets = 1;

    [BoxGroup("Zielerfassung")]
    public float maxDuration = 20f;

    [BoxGroup("Zielerfassung")]
    [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
    [Range(0, 10)]
    public float multiHitDelay = 0;

    [BoxGroup("Zielerfassung")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showRange = false;

    [BoxGroup("Zielerfassung")]
    public RangeType rangeType = RangeType.circle;

    [BoxGroup("Zielerfassung")]
    public float range = 6f;

    [BoxGroup("Indikator")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showIndicator = false;

    [BoxGroup("Indikator")]
    [ShowIf("showIndicator")]
    [SerializeField]
    private Indicator indicator;

    [BoxGroup("Indikator")]
    [ShowIf("showIndicator")]
    [SerializeField]
    private bool overrideColor = false;

    [BoxGroup("Indikator")]
    [ShowIf("overrideColor")]
    [SerializeField]
    private Color color = Color.white;

    public void Instantiate(Character sender, Character target, List<Indicator> appliedIndicators)
    {
        if (this.showIndicator && !alreadyApplied(sender, target, appliedIndicators)) //check if already applied
        {
            Indicator indicator = Instantiate(this.indicator);
            indicator.Initialize(sender, target);
            indicator.name = this.indicator.name;
            if (this.overrideColor) indicator.SetColor(this.color);
            appliedIndicators.Add(indicator);
        }
    }

    private bool alreadyApplied(Character sender, Character target, List<Indicator> appliedIndicators)
    {
        foreach(Indicator applied in appliedIndicators)
        {
            if (applied.GetSender() == sender && applied.GetTarget() == target) return true;
        }

        return false;
    }
}
