using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectile : SkillExtension
{
    private bool speedup = true;
    private Vector2 tempVelocity;

    private void Update()
    {
        if (this.skill.delayTimeLeft <= 0 && this.skill.isActive) setVelocity();
    }    

    private void setVelocity()
    {
        if (this.skill.myRigidbody != null && this.speedup)
        {
            this.skill.myRigidbody.velocity = this.skill.direction.normalized * this.skill.speed;
            this.tempVelocity = this.skill.myRigidbody.velocity;
            this.speedup = false;
        }
    }        
}
