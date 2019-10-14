using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSelfHit : SkillHitTrigger
{
    [InfoBox("Wirkt auf den Sender direkt (ohne Collider)")]
    [SerializeField]
    [Range(0, 10)]
    private float immortalTimer = 0;


    private void Start()
    {
        if (this.immortalTimer > 0) this.skill.sender.setImmortal(this.immortalTimer);
        this.skill.sender.gotHit(this.skill);
    }
}
