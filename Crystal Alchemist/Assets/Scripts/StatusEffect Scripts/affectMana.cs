using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectMana : StatusEffect
{
    #region Attributes
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float mana;
    #endregion


    #region Overrides
    public override void doEffect()
    {
        base.doEffect();
        this.target.updateMana(this.mana);
    }
    #endregion
}
