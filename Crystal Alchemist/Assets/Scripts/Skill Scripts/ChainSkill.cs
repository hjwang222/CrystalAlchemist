using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChainSkill : StandardSkill
{
    [FoldoutGroup("Special Behaviors", expanded: false)]
    public bool useRange = false;

    [SerializeField]
    [ShowIf("useRange", true)]
    [FoldoutGroup("Special Behaviors", expanded: false)]
    private Vector2 rangeNeeded;


    [SerializeField]
    [HideIf("useRange", true)]
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [Range(0, Utilities.maxFloatSmall)]
    private float distanceNeeded = 0f;

    [HideIf("useRange", true)]
    [FoldoutGroup("Special Behaviors", expanded: false)]
    public bool canBreak = false;

    [HideIf("useRange", true)]
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private bool useStartDistance = false;

    [Space(10)]
    [FoldoutGroup("Special Behaviors", expanded: false)]
    public SpriteRenderer chainSpriteRenderer;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    public bool changeColor = false;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [ShowIf("changeColor", true)]
    public Color rightColor;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [ShowIf("changeColor", true)]
    public Color wrongColor;

    private float startDistance = 0;


    public override void initializeIndicator(Indicator indicator)
    {
        base.initializeIndicator(indicator);

        if (this.chainSpriteRenderer != null && this.activeIndicator != null)
        {
            indicator.indicatorRenderer.sprite = this.chainSpriteRenderer.sprite;
            indicator.indicatorRenderer.color = this.chainSpriteRenderer.color;
            indicator.indicatorRenderer.size = new Vector2(indicator.indicatorRenderer.size.x, this.chainSpriteRenderer.size.y);

            indicator.animator.enabled = false;
        }
    }

    public override void doOnCast()
    {
        if (this.hasRightDistance())
        {
            if (this.changeColor)
            {
                foreach (Indicator indicator in this.indicators)
                {
                    indicator.indicatorRenderer.color = this.rightColor;
                }
            }

            if (this.canBreak && !this.useRange)
            {
                hideIndicator();
                this.DestroyIt();
            }
        }
        else
        {
            if (this.changeColor) foreach (Indicator indicator in this.indicators)
                {
                    indicator.indicatorRenderer.color = this.wrongColor;
                }
        }

    }


    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        checkMechanics(hittedCharacter);
    }

    public override void OnTriggerStay2D(Collider2D hittedCharacter)
    {

    }

    private void checkMechanics(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this))
        {
            //update Target Resource Affections
            if (!this.hasRightDistance()) landAttack(hittedCharacter);
        }
    }

    public bool hasRightDistance()
    {
        if (this.target != null)
        {
            if (this.useStartDistance && this.startDistance <= 0)
                this.startDistance = Vector3.Distance(this.target.transform.position, this.sender.transform.position);

            return Utilities.Collisions.checkDistance(this.target,
                                                      this.sender.gameObject, rangeNeeded.x, rangeNeeded.y,
                                                      this.startDistance, this.distanceNeeded,
                                                      this.useStartDistance, this.useRange);
        }

        return false;
    }
}
