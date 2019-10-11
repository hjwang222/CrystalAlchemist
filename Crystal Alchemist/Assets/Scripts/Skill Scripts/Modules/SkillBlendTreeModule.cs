using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBlendTreeModule : SkillModule
{
    [BoxGroup("Blend Tree")]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    public bool useOffSetToBlendTree = false;
}
