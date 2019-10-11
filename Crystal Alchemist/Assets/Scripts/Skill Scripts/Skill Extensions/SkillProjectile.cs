﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    private bool speedup = true;
    private Vector2 tempVelocity;

    private void Update()
    {
        if (this.skill.delayTimeLeft <= 0) setVelocity();
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

    public void updateTimeDistortion(float distortion)
    {
        if (this.skill.myRigidbody != null && this.skill.isActive) this.skill.myRigidbody.velocity = this.skill.direction.normalized * this.skill.speed * this.skill.timeDistortion;
    }   
}
