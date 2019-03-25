using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectSpeed : StatusEffect
{
    //STATUSEFFEKT SCRIPT "SPEED UPDATE"
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float speed;

    public override void doEffect()
    {
        base.doEffect();
        this.target.updateSpeed(this.speed); 
    }
}
