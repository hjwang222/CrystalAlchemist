using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDistortion : StatusEffect
{
    //STATUSEFFEKT SCRIPT "ZEIT STASE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float time;

    public new void DestroyIt()
    {
        //Zeit wieder normalisieren
        target.updateTimeDistortion(0);
        base.DestroyIt();
    }

    public override void init()
    {
        base.init();
        //Charakter mit einem Zeitdebuff versehen
        target.updateTimeDistortion(this.time);   
    }
}
