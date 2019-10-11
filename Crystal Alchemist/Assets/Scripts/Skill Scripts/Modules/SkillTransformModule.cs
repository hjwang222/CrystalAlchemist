using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTransformModule : SkillModule
{
    [Space(10)]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    public bool rotateIt = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    public bool rotateOnUpdate = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    [HideIf("rotateIt")]
    public bool keepOriginalRotation = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    [HideIf("rotateIt")]
    public bool blendTree = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    [HideIf("rotateIt")]
    public bool useOffSetToBlendTree = false;

    [ShowIf("rotateIt")]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Range(-360, 360)]
    public float rotationModifier = 0;

    [ShowIf("rotateIt")]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
    [Range(0, 90)]
    public float snapRotationInDegrees = 0f;

    [Space(10)]
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Range(0, 2)]
    [Tooltip("Positions-Offset, damit es nicht im Character anfängt")]
    public float positionOffset = 1f;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Range(0, 2)]
    [Tooltip("Positions-Höhe")]
    public float positionHeight = 0f;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Schattencollider Höhe")]
    [Range(-1, 0)]
    public float colliderHeightOffset = 0;
    
    private Quaternion fixedRotation = Quaternion.Euler(0, 0, 0);

    [HideInInspector]
    public bool setPositionAtStart = true;



    private void Update()
    {
        if (this.rotateOnUpdate) this.skill.setPostionAndDirection();

        if (this.skill.spriteRenderer != null && this.skill.shadow != null)
        {
            if (!this.keepOriginalRotation) this.skill.shadow.transform.rotation = this.skill.spriteRenderer.transform.rotation;
        }
    }

    public void LateUpdate()
    {
        if (!this.rotateIt && !this.keepOriginalRotation) this.transform.rotation = this.fixedRotation;
    }

    
}
