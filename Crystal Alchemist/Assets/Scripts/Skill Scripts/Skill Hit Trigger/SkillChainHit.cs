using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillChainHit : SkillExtension
{
    [BoxGroup("Special Behaviors")]
    public bool useRange = false;

    [SerializeField]
    [ShowIf("useRange", true)]
    [BoxGroup("Special Behaviors")]
    private Vector2 rangeNeeded;

    [SerializeField]
    [HideIf("useRange", true)]
    [BoxGroup("Special Behaviors")]
    [Range(0, CustomUtilities.maxFloatSmall)]
    private float distanceNeeded = 0f;

    [HideIf("useRange", true)]
    [BoxGroup("Special Behaviors")]
    public bool canBreak = false;

    [HideIf("useRange", true)]
    [BoxGroup("Special Behaviors")]
    [SerializeField]
    private bool useStartDistance = false;

    [BoxGroup("Special Behaviors")]
    public bool changeColor = false;

    [BoxGroup("Special Behaviors")]
    [ShowIf("changeColor", true)]
    public Color rightColor;

    [BoxGroup("Special Behaviors")]
    [ShowIf("changeColor", true)]
    public Color wrongColor;

    private float startDistance = 0;

    [BoxGroup("Special Behaviors")]
    [SerializeField]
    [Required]
    private SkillIndicatorModule indicatorModule;

    public void doOnCast()
    {
        if (this.hasRightDistance())
        {
            if (this.changeColor) this.indicatorModule.activeIndicator.indicatorRenderer.color = this.rightColor;  

            if (this.canBreak && !this.useRange)
            {
                this.indicatorModule.hideIndicator();
                this.skill.DestroyIt();
            }
        }
        else
        {
            if (this.changeColor) this.indicatorModule.activeIndicator.indicatorRenderer.color = this.wrongColor;            
        }
    }


    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        checkMechanics(hittedCharacter);
    }

    private void checkMechanics(Collider2D hittedCharacter)
    {
        if (CustomUtilities.Collisions.checkCollision(hittedCharacter, this.skill))
        {
            //update Target Resource Affections
            if (!this.hasRightDistance()) this.skill.hitIt(hittedCharacter);
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
