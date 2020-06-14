using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSelfHit : SkillHitTrigger
{
    [InfoBox("Wirkt auf den Sender direkt (ohne Collider)")]
    [SerializeField]
    [MinValue(0)]
    private float invincibleTimer = 0;

    public override void Initialize()
    {
        if (this.invincibleTimer > 0) this.skill.sender.SetInvincible(this.invincibleTimer, false);
        this.skill.sender.gotHit(this.skill);
    }
}
