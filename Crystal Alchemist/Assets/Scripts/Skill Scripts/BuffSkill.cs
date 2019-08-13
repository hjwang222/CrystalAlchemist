using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : StandardSkill
{
    #region Overrides
    public override void init()
    {
        base.init();
        if (this.affectSelf) UseSkillOnSelf(); 
    }
    #endregion


    #region Functions (private)
    public void UseSkillOnSelf()
    {
        //Spieler selbst (Heals, Buffs)
        this.sender.gotHit(this);
    }
    #endregion
}

