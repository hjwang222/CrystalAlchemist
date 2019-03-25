using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectMana : StatusEffect
{
    //STATUSEFFEKT SCRIPT "MANA UPDATE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float mana;

    public override void doEffect()
    {
        base.doEffect();
        this.target.updateMana(this.mana);
    }
}
