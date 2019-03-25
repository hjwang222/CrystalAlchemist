using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectMana : Script
{
    //STATUSEFFEKT SCRIPT "MANA UPDATE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float mana;

    public override void onDestroy()
    {

    }

    public override void onUpdate()
    {
        this.target.updateMana(this.mana);
    }

    public override void onInitialize()
    {

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
}
