using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectSpeed : StatusEffect
{
    #region Attributes
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float speed;
    #endregion


    #region Overrides
    public override void doEffect()
    {
        base.doEffect();
        this.target.updateSpeed(this.speed); 
    }
    #endregion
}
