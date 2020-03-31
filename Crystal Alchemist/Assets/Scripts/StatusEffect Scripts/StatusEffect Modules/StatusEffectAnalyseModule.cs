using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectAnalyseModule : StatusEffectModule
{
    [SerializeField]
    private BoolSignal signal;

    public override void doAction()
    {
        this.signal.Raise(false);
    }

    private void OnDestroy()
    {
        this.signal.Raise(false);
    }
}
