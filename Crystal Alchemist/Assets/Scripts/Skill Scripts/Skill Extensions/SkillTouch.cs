using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTouch : SkillExtension
{
    [SerializeField]
    [Required]
    private BoxCollider2D boxCollider;

    private void Start()
    {    
        this.boxCollider.offset = this.skill.sender.boxCollider.offset;
        this.boxCollider.size = this.skill.sender.boxCollider.size;
    }
}
