using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBlendTreeModule : SkillModule
{
    [BoxGroup("Blend Tree")]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    public bool useOffSetToBlendTree = false;
}
