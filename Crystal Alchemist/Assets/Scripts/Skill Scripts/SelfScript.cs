using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfScript : Script
{
    public override void onDestroy()
    {

    }

    public override void onUpdate()
    {
        
    }

    public override void onInitialize()
    {
        if (this.skill.affectSelf) UseSkillOnSelf(); //ON SCRIPT! FÜR HEALS!
    }

    public override void onExit(Collider2D hittedCharacter)
    {

    }

    public override void onStay(Collider2D hittedCharacter)
    {
        
    }

    public override void onEnter(Collider2D hittedCharacter)
    {

    }

    private void moveToTarget()
    {
        
    }

    public void UseSkillOnSelf()
    {
        //Spieler selbst (Heals, Buffs)
        this.skill.sender.gotHit(this.skill);
    }
}

