using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillChain : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

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
    private SkillIndicatorModule indicatorModule;

    private void Start()
    {
        this.indicatorModule = this.GetComponent<SkillIndicatorModule>();
    }

    public void initializeIndicator(Indicator indicator)
    {
        //base.initializeIndicator(indicator);

        if (this.indicatorModule != null
            && this.chainSpriteRenderer != null
            && this.indicatorModule.activeIndicators != null)
        {
            indicator.indicatorRenderer.sprite = this.chainSpriteRenderer.sprite;
            indicator.indicatorRenderer.color = this.chainSpriteRenderer.color;
            indicator.indicatorRenderer.size = new Vector2(indicator.indicatorRenderer.size.x, this.chainSpriteRenderer.size.y);

            indicator.animator.enabled = false;
        }
    }

    public void doOnCast()
    {
        if (this.hasRightDistance())
        {
            if (this.changeColor)
            {
                foreach (Indicator indicator in this.indicatorModule.indicators)
                {
                    indicator.indicatorRenderer.color = this.rightColor;
                }
            }

            if (this.canBreak && !this.useRange)
            {
                this.indicatorModule.hideIndicator();
                this.skill.DestroyIt();
            }
        }
        else
        {
            if (this.changeColor) foreach (Indicator indicator in this.indicatorModule.indicators)
                {
                    indicator.indicatorRenderer.color = this.wrongColor;
                }
        }

    }


    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        checkMechanics(hittedCharacter);
    }

    private void checkMechanics(Collider2D hittedCharacter)
    {
        if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill))
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

            return Utilities.Collisions.checkDistance(this.skill.target,
                                                      this.skill.sender.gameObject, rangeNeeded.x, rangeNeeded.y,
                                                      this.startDistance, this.distanceNeeded,
                                                      this.useStartDistance, this.useRange);
        }

        return false;
    }
}
