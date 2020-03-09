using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillChainHit : SkillMechanicHit
{
    [BoxGroup("Mechanics")]
    public bool useRange = false;

    [SerializeField]
    [ShowIf("useRange", true)]
    [BoxGroup("Mechanics")]
    private Vector2 rangeNeeded;

    [SerializeField]
    [HideIf("useRange", true)]
    [BoxGroup("Mechanics")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    private float distanceNeeded = 0f;

    [HideIf("useRange", true)]
    [BoxGroup("Mechanics")]
    public bool canBreak = false;

    [HideIf("useRange", true)]
    [BoxGroup("Mechanics")]
    [SerializeField]
    private bool useStartDistance = false;

    [BoxGroup("Mechanics")]
    public bool changeColor = false;

    [BoxGroup("Mechanics")]
    [ShowIf("changeColor", true)]
    public Color rightColor;

    [BoxGroup("Mechanics")]
    [ShowIf("changeColor", true)]
    public Color wrongColor;

    private float startDistance = 0;

    [BoxGroup("Mechanics")]
    [SerializeField]
    [Required]
    private SkillIndicatorModule indicatorModule;

    public void doOnCast()
    {
        if (this.hasRightDistance())
        {
            this.percentage = 0;
            if (this.changeColor) this.indicatorModule.activeIndicator.indicatorRenderer.color = this.rightColor;  

            if (this.canBreak && !this.useRange)
            {
                this.indicatorModule.hideIndicator();
                this.skill.DestroyIt();
            }
        }
        else
        {
            this.percentage = 100;
            if (this.changeColor) this.indicatorModule.activeIndicator.indicatorRenderer.color = this.wrongColor;            
        }
    }

    public bool hasRightDistance()
    {
        if (this.skill.target != null)
        {
            if (this.useStartDistance && this.startDistance <= 0)
                this.startDistance = Vector3.Distance(this.skill.target.transform.position, this.skill.sender.transform.position);

            return CustomUtilities.Collisions.checkDistance(this.skill.target,
                                                      this.skill.sender.gameObject, rangeNeeded.x, rangeNeeded.y,
                                                      this.startDistance, this.distanceNeeded,
                                                      this.useStartDistance, this.useRange);
        }

        return false;
    }
}
