using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSummon : SkillExtension
{
    [SerializeField]
    private Character summon;

    [SerializeField]
    [Range(0,6)]
    private float immortalTimerPet = 0;

    private void Start()
    {
        summoning();
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
            pet.direction = this.skill.direction;
            pet.partner = this.skill.sender;
            if (this.immortalTimerPet > 0) pet.setImmortalAtStart(this.immortalTimerPet);

            this.skill.sender.activePets.Add(pet);
        }
        else if (breakable != null)
        {
            Breakable objectPet = Instantiate(breakable, this.transform.position, Quaternion.Euler(0, 0, 0));
            objectPet.direction = this.skill.direction;
            objectPet.changeAnim(objectPet.direction);
            if (this.immortalTimerPet > 0) objectPet.setImmortalAtStart(this.immortalTimerPet);
        }        
    }
}
