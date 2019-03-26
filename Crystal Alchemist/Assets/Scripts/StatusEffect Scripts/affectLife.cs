using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectLife : StatusEffect
{
    #region Attributes
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float life;
    #endregion


    #region Overrides
    public override void doEffect()
    {
        base.doEffect();
        this.target.updateLife(this.life);
    }
    #endregion
}
