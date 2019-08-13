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

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private SimpleSignal musicPitchSignal;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private float musicPitch;

    public override void init()
    {
        base.init();
        GlobalValues.backgroundMusicPitch = this.musicPitch;
        this.musicPitchSignal.Raise();
    }

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
        GlobalValues.backgroundMusicPitch = 1f;
        this.musicPitchSignal.Raise();
        this.sender.resetColor(this.targetColor);
    }

}
