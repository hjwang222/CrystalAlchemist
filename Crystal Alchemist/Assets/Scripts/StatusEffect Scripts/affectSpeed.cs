using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectSpeed : StatusEffect
{
    #region Attributes
    [SerializeField]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    private float speed;

    #endregion


    #region Overrides
    public override void doEffect()
    {
        base.doEffect();
        this.target.updateSpeed(this.speed);
    }

    public override void DestroyIt()
    {
        //Zeit wieder normalisieren
        this.target.updateSpeed(0);
        base.DestroyIt();
    }
    #endregion
}
