using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectSpeed : Script
{
    //STATUSEFFEKT SCRIPT "SPEED UPDATE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float speed;

    public override void onDestroy()
    {

    }

    public override void onUpdate()
    {
        this.target.updateSpeed(this.speed); 
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
