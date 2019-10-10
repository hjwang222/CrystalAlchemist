using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SummonSkill : StandardSkill
{
    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private Character summon;

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
        return this.summon.stats.characterName;
    }

    private void summoning()
    {
        AI ai = this.summon.GetComponent<AI>();
        Breakable breakable = this.summon.GetComponent<Breakable>();

        if (ai != null)
        {
            AI pet = Instantiate(ai, this.transform.position, Quaternion.Euler(0, 0, 0));
            pet.direction = this.direction;
            pet.partner = this.sender;
            this.sender.activePets.Add(pet);
        }
        else if(breakable != null)
        {
            Breakable objectPet = Instantiate(breakable, this.transform.position, Quaternion.Euler(0, 0, 0));
            objectPet.direction = this.direction;
            objectPet.changeAnim(objectPet.direction);
        }
    }
}
