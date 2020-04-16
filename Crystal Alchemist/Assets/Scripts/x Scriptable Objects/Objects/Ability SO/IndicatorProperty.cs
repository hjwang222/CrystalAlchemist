using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ability/Indicator Property")]
public class IndicatorProperty : ScriptableObject
{
    [BoxGroup("Indikator")]
    [SerializeField]
    public Indicator indicator;

    [BoxGroup("Indikator")]
    [SerializeField]
    public bool overrideColor = false;

    [BoxGroup("Indikator")]
    [ShowIf("overrideColor")]
    [SerializeField]
    public Color color = Color.white;

    public void Instantiate(Character sender, Character target, List<Indicator> appliedIndicators)
    {
        if (!alreadyApplied(sender, target, appliedIndicators)) //check if already applied
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
        foreach (Indicator applied in appliedIndicators)
        {
            if (applied.GetSender() == sender && applied.GetTarget() == target) return true;
        }

        return false;
    }
}
