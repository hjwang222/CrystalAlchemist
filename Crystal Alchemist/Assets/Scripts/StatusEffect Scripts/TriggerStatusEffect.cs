using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TriggerStatusEffect : StatusEffect
{
    //TODO: Verschiedene Trigger und Aktionen
    //TODO: Dispell-Varianten (Anti, Hit, etc)
    //TODO: Status-Effekt Module

    [SerializeField]
    private List<StandardSkill> actions = new List<StandardSkill>();

    public override void init()
    {
        base.init();
        this.target.cannotDie = true;
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();

        if(this.target.life <= 0)
        {
            Utilities.Skill.instantiateSkill(this.actions[0], this.target);
            this.DestroyIt();
        }
    }

    private void OnDestroy()
    {
        this.target.cannotDie = false;
    }
}
