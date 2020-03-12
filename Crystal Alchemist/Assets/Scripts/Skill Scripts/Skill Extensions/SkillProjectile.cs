using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectile : SkillExtension
{
    private Vector2 tempVelocity;

    public float speed = 1;


    public virtual void Start()
    {
        if (this.skill.myRigidbody != null)
        {
            this.setVelocity();
            this.tempVelocity = this.skill.myRigidbody.velocity;
        }
    }

    public void setVelocity()
    {
        if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = this.skill.direction.normalized * this.speed * this.skill.getTimeDistortion();
    }

    public void stopVelocity()
    {
        if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = Vector2.zero;
    }
}
