using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IndicatorObject
{
    [BoxGroup("Indikator")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public bool showIndicator = false;

    [BoxGroup("Indikator")]
    [ShowIf("showIndicator")]
    [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
    public IndicatorProperty indicatorProperty;

    private List<Indicator> appliedIndicators = new List<Indicator>();

    private void Instantiate(Character sender, Character target)
    {
        if (this.showIndicator && this.indicatorProperty != null) this.indicatorProperty.Instantiate(sender, target, this.appliedIndicators);
    }

    public void UpdateIndicator(Character sender, Character target)
    {
        if (this.showIndicator)
        {
            if (target != null) Instantiate(sender, target);
            else ClearIndicator();
        }
    }

    public void UpdateIndicator(Character sender, List<Character> selectedTargets, TargetingMode mode)
    {
        if (this.showIndicator)
        {
            if (mode != TargetingMode.helper)
            {
                this.RemoveIndicator(selectedTargets);
                AddIndicator(sender, selectedTargets);                
            }
            else
            {
                this.Instantiate(sender, null);
            }
        }
    }

    private void AddIndicator(Character sender, List<Character> selectedTargets)
    {
        foreach (Character target in selectedTargets)
        {
            this.Instantiate(sender, target);
        }
    }

    public void RemoveIndicator(List<Character> selectedTargets)
    {
        List<Indicator> tempAppliedList = new List<Indicator>();

        foreach (Indicator applied in this.appliedIndicators)
        {
            if (applied != null && !selectedTargets.Contains(applied.GetTarget())) Object.Destroy(applied.gameObject);
            else tempAppliedList.Add(applied);
        }

        this.appliedIndicators = tempAppliedList;
        this.appliedIndicators.RemoveAll(item => item == null);
    }

    public void ClearIndicator()
    {
        foreach (Indicator applied in this.appliedIndicators)
        {
            Object.Destroy(applied.gameObject);
        }
        this.appliedIndicators.Clear();
    }
}
