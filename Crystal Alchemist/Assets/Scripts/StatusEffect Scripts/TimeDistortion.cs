using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDistortion : Script
{
    //STATUSEFFEKT SCRIPT "ZEIT STASE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float time;

    public override void onDestroy()
    {
        //Zeit wieder normalisieren
        target.updateTimeDistortion(0);
    }

    public override void onUpdate()
    {

    }

    public override void onInitialize()
    {
        //Charakter mit einem Zeitdebuff versehen
            target.updateTimeDistortion(this.time);   
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
