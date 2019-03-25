using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectLife : Script
{
    //STATUSEFFEKT SCRIPT "LIFE UPDATE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float life;

    public override void onDestroy()
    {
        
    }

    public override void onUpdate()
    {
        this.target.updateLife(this.life);
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
