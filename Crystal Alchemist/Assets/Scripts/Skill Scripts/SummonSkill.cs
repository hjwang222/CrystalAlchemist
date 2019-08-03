using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SummonSkill : StandardSkill
{
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private AI summon;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [Tooltip("true = beim Start, ansonsten nach Delay")]
    [SerializeField]
    private bool summonInstantly = true;

    public override void init()
    {
        base.init();

        if (this.summonInstantly) summoning();
    }

    public override void doOnUpdate()
    {
        base.doOnUpdate();

        if (this.delayTimeLeft <= 0 && !this.summonInstantly)
        {
            summoning();
        }
    }

    public string getPetName()
    {
        return this.summon.characterName;
    }

    private void summoning()
    {
        AI pet = Instantiate(this.summon, this.transform.position, Quaternion.Euler(0, 0, 0));
        pet.partner = this.sender;
        this.sender.activePets.Add(pet);
    }
}
