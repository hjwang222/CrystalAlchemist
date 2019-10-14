﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillBoomerang : SkillExtension
{
    #region Attributes
    [Tooltip("Zeitpunkt der Scriptaktivierung")]
    [Range(0, Utilities.maxFloatSmall)]
    public float timeToMoveBack = 0;

    [HideInInspector]
    public float durationThenBackToSender = 0;

    [SerializeField]
    private float minDistance = 0.1f;

    private bool speedup = true;
    private Vector2 tempVelocity;
    #endregion
    

    private void Start()
    {
        this.durationThenBackToSender = timeToMoveBack;
    }

    private void Update()
    {
        if (this.durationThenBackToSender > 0)
        {
            this.durationThenBackToSender -= (Time.deltaTime * this.skill.timeDistortion);
        }
        else
        {
            moveBackToSender();
        }
    }

    #region Functions (private)
    private void moveBackToSender()
    {
        if (this.skill.sender != null)
        {
            //Bewege den Skill zurück zum Sender

            this.skill.myRigidbody.velocity = Vector2.zero;
            if (Vector3.Distance(this.skill.sender.transform.position, this.transform.position) > this.minDistance)
            {
                Vector3 newPosition = Vector3.MoveTowards(this.transform.position, this.skill.sender.transform.position, this.skill.speed * (Time.deltaTime * this.skill.timeDistortion));

                this.skill.myRigidbody.MovePosition(newPosition);
                this.skill.myRigidbody.velocity = Vector2.zero;                
            }
            else
            {
                this.skill.DestroyIt();
            }
        }
    }
    #endregion
}