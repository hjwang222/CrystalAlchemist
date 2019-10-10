using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTransformModule : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

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
    [SerializeField]
    private bool blendTree = false;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Wird ein Blend-Tree verwendet (Animation)?")]
    [HideIf("rotateIt")]
    [SerializeField]
    private bool useOffSetToBlendTree = false;

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
    [Tooltip("Schattencollider")]
    [Range(-1, 0)]
    public float colliderHeightOffset = 0;


    private Quaternion fixedRotation = Quaternion.Euler(0, 0, 0);
    private bool setPositionAtStart = true;


    private void Update()
    {
        if (this.rotateOnUpdate) setPostionAndDirection();

        if (this.skill.spriteRenderer != null && this.skill.shadow != null)
        {
            if (!this.keepOriginalRotation) this.skill.shadow.transform.rotation = this.skill.spriteRenderer.transform.rotation;
        }
    }

    public void LateUpdate()
    {
        if (!this.rotateIt && !this.keepOriginalRotation) this.transform.rotation = this.fixedRotation;
    }

    private void setPostionAndDirection()
    {
        //Bestimme Winkel und Position

        float angle = 0;
        Vector2 start = this.transform.position;
        Vector3 rotation = this.transform.rotation.eulerAngles;

        if (!this.blendTree)
        {
            if (!this.keepOriginalRotation)
            {
                Utilities.Rotation.setDirectionAndRotation(this.skill.sender, this.skill.target,
             this.positionOffset, this.positionHeight, this.snapRotationInDegrees, this.rotationModifier,
             out angle, out start, out this.skill.direction, out rotation);
            }

            //if (this.target != null) this.direction = (Vector2)this.target.transform.position - start;                       

            if (setPositionAtStart) this.transform.position = start;

            if (this.keepOriginalRotation)
            {
                this.skill.direction = Utilities.Rotation.DegreeToVector2(this.transform.rotation.eulerAngles.z);
            }

            if (this.rotateIt && !this.keepOriginalRotation) transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            if (this.useOffSetToBlendTree) this.transform.position = new Vector2(this.skill.sender.transform.position.x + (this.skill.sender.direction.x * positionOffset),
                                                                                 this.skill.sender.transform.position.y + (this.skill.sender.direction.y * positionOffset) + positionHeight);
        }

        if (this.skill.animator != null)
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "moveX", this.skill.sender.direction.x);
            Utilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "moveY", this.skill.sender.direction.y);
        }

        if (this.skill.shadow != null)
        {
            float changeX = 0;
            if (this.skill.direction.y < 0) changeX = this.skill.direction.y;
            this.skill.shadow.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + this.colliderHeightOffset + (this.colliderHeightOffset * changeX));
        }
    }
}
