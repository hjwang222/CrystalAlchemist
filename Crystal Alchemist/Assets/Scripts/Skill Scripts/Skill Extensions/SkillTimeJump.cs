using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTimeJump : SkillExtension
{
    [SerializeField]
    private TimeValue timeValue;
      
    [SerializeField]
    private float newValue = 0.05f;

    [SerializeField]
    private Color targetColor;

    [SerializeField]
    private SimpleSignal musicPitchSignal;

    [SerializeField]
    private float musicPitch;


    private void Start()
    {
        GlobalValues.backgroundMusicPitch = this.musicPitch;
        this.musicPitchSignal.Raise();
    }

    private void Update()
    {
        this.timeValue.factor = this.newValue;

        if (this.targetColor != null)
        {
            this.skill.sender.addColor(this.targetColor);
        }
    }
    
    private void OnDestroy()
    {
        this.timeValue.factor = this.timeValue.normalFactor;
        GlobalValues.backgroundMusicPitch = 1f;
        this.musicPitchSignal.Raise();
        this.skill.sender.resetColor(this.targetColor);
    }
}
