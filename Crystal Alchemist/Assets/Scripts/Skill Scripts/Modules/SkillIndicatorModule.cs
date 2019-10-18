using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillIndicatorModule : SkillModule
{
    [FoldoutGroup("Indikatoren", expanded: false)]
    [Tooltip("AOE")]
    [SerializeField]
    private Indicator indicator;

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
    public Indicator activeIndicator;

    public void hideIndicator()
    {
        if (this.indicator != null
            && this.activeIndicator != null
            && this.showingIndicator)
        {
            this.activeIndicator.DestroyIt();
            this.activeIndicator = null;
        }
    }

    public void showIndicator()
    {
        if (this.indicator != null
            && this.activeIndicator == null
            && this.showingIndicator)
        {
            Indicator temp = Instantiate(this.indicator);
            temp.setSkill(this.skill);
            this.activeIndicator = temp;
        }
    }
}
