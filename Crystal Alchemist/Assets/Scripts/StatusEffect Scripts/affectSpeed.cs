using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class affectSpeed : StatusEffect
{
    #region Attributes
    [SerializeField]
    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    private float speed;

    [SerializeField]
    private Animator anim;
    #endregion


    #region Overrides
    public override void doEffect()
    {
        base.doEffect();
        this.target.updateSpeed(this.speed);
        if(this.anim != null) this.anim.speed = 1;        
    }

    public override void DestroyIt()
    {
        //Zeit wieder normalisieren
        this.target.updateSpeed(0);
        base.DestroyIt();
    }
    #endregion
}
