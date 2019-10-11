using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTimeJump : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

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
