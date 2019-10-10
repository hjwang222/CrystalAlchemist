using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [Tooltip("Geschwindigkeit des Projektils")]
    [Range(0, Utilities.maxFloatSmall)]
    public float speed = 0;

    public void updateTimeDistortion(float distortion)
    {
        if (this.skill.myRigidbody != null && this.skill.isActive) this.skill.myRigidbody.velocity = this.skill.direction.normalized * this.speed * this.skill.timeDistortion;
    }

   
}
