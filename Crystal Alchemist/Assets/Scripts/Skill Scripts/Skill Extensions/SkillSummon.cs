using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSummon : SkillExtension
{
    [SerializeField]
    private Character summon;

    private void Start()
    {
        summoning();
    }

    public string getPetName()
    {
        return this.summon.stats.GetCharacterName();
    }

    private void summoning()
    {
        AI ai = this.summon.GetComponent<AI>();
        Breakable breakable = this.summon.GetComponent<Breakable>();

        if (ai != null)
        {
            AI pet = Instantiate(ai, this.transform.position, Quaternion.Euler(0, 0, 0));
            pet.name = ai.name;
            pet.direction = this.skill.direction;
            pet.partner = this.skill.sender;

            this.skill.sender.activePets.Add(pet);
        }
        else if (breakable != null)
        {
            Breakable objectPet = Instantiate(breakable, this.transform.position, Quaternion.Euler(0, 0, 0));
            objectPet.direction = this.skill.direction;
            objectPet.changeAnim(objectPet.direction);
        }        
    }
}
