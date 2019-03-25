using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectLife : StatusEffect
{
    //STATUSEFFEKT SCRIPT "LIFE UPDATE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float life;

    public override void doEffect()
    {
        base.doEffect();
        this.target.updateLife(this.life);
    }
}
