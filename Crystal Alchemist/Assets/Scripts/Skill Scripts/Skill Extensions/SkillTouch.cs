using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTouch : SkillExtension
{
    private void Start()
    {
        this.GetComponent<BoxCollider2D>().offset = this.skill.sender.boxCollider.offset;
        this.GetComponent<BoxCollider2D>().size = this.skill.sender.boxCollider.size;
    }
}
