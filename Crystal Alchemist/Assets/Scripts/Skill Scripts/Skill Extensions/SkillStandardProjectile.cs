using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStandardProjectile : SkillExtension
{
    public float speed;

    public virtual void setVelocity()
    {
        if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = this.skill.direction.normalized * this.speed * this.skill.timeDistortion;
    }

    public virtual void stopVelocity()
    {
        if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = Vector2.zero;
    }
}
