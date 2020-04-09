using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillRotationModule : SkillModule
{
    [BoxGroup("Rotations")]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    public bool rotateIt = false;

    [BoxGroup("Rotations")]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    public bool rotateOnUpdate = false;

    [BoxGroup("Rotations")]
    [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
    [HideIf("rotateIt")]
    public bool keepOriginalRotation = false;

    [BoxGroup("Rotations")]
    [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
    [Range(0, 90)]
    public float snapRotationInDegrees = 0f;
    
    private Quaternion fixedRotation = Quaternion.Euler(0, 0, 0);
          

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
