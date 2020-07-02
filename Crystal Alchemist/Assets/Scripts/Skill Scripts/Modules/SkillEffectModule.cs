using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectModule : SkillModule
{
    [SerializeField]
    private GameObject effect;

    public override void Initialize()
    {
        base.Initialize();
        Instantiate(this.effect, this.transform.position, Quaternion.identity);
    }
}
