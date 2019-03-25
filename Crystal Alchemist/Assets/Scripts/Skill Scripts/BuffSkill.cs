using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : StandardSkill
{
    public override void init()
    {
        base.init();
        if (this.affectSelf) UseSkillOnSelf(); 
    }

    public void UseSkillOnSelf()
    {
        //Spieler selbst (Heals, Buffs)
        this.sender.gotHit(this);
    }
}

