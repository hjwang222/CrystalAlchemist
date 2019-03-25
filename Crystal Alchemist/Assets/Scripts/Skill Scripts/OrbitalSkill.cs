using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalSkill : StandardSkill
{
    public override void doOnUpdate()
    {
        base.doOnUpdate();
        if (this.target != null) this.transform.position = this.target.transform.position;
    }

    public override void init()
    {
        base.init();
        if (this.target != null) this.transform.position = this.target.transform.position;
    }
}
