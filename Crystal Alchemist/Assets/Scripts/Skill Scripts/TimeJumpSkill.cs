using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TimeJumpSkill : StandardSkill
{
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private TimeValue timeValue;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private float newValue = 0.05f;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private Color targetColor;

    public override void doOnUpdate()
    {
        base.doOnUpdate();
        this.timeValue.factor = this.newValue;

        if (this.targetColor != null)
        {
            this.sender.addColor(this.targetColor);
        }
    }

    private void OnDestroy()
    {
        this.timeValue.factor = this.timeValue.normalFactor;

        this.sender.resetColor(this.targetColor);
    }

}
