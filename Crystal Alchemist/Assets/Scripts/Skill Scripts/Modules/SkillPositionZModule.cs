using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillPositionZModule : SkillModule
{
    [BoxGroup("Position Z")]
    [Tooltip("Position anhand GameObject verwenden")]
    public bool useGameObjectHeight = false;

    [BoxGroup("Position Z")]
    [Range(0, 2)]
    [Tooltip("Positions-Höhe")]
    [HideIf("useGameObjectHeight")]
    public float positionHeight = 0f;

    [BoxGroup("Position Z")]
    [Tooltip("Schattencollider Höhe")]
    [Range(-1, 0)]
    public float colliderHeightOffset = -0.5f;


}
