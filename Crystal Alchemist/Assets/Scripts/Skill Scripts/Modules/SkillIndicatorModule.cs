using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillIndicatorModule : SkillModule
{
    [FoldoutGroup("Indikatoren", expanded: false)]
    [Tooltip("AOE")]
    public List<Indicator> indicators = new List<Indicator>();

    [FoldoutGroup("Indikatoren", expanded: false)]
    [Tooltip("Indikator anzeigen")]
    public bool showingIndicator = true;

    [FoldoutGroup("Indikatoren", expanded: false)]
    [Tooltip("Cast anzeigen")]
    public bool showCastBarForEnemies = true;

    [FoldoutGroup("Indikatoren", expanded: false)]
    public bool useCustomColor;

    [FoldoutGroup("Indikatoren", expanded: false)]
    [ShowIf("useCustomColor", true)]
    public Color indicatorColor;


    [HideInInspector]
    public List<Indicator> activeIndicators = new List<Indicator>();

    public void hideIndicator()
    {
        foreach (Indicator indicator in this.activeIndicators)
        {
            if (indicator != null) indicator.DestroyIt();
        }

        this.activeIndicators.Clear();
    }

    public void showIndicator()
    {
        if (this.indicators.Count > 0
            && this.activeIndicators.Count == 0
            && this.showingIndicator)
        {
            foreach (Indicator indicator in this.indicators)
            {
                Indicator temp = Instantiate(indicator);
                temp.setSkill(this.skill);
                this.activeIndicators.Add(temp);
            }
        }
    }
}
