using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDistortion : StatusEffect
{
    #region Attributes
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float time;
    #endregion


    #region Overrides
    public override void DestroyIt()
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
    #endregion
}
