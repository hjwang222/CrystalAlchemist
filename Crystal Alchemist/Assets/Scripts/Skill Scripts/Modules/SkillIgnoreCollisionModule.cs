using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillIgnoreCollisionModule : SkillModule
{
    [SerializeField]
    private Collider2D ignoredCollider2D;

    void Start()
    {
        Physics2D.IgnoreCollision(this.skill.sender.boxCollider, this.ignoredCollider2D);
    }
}
