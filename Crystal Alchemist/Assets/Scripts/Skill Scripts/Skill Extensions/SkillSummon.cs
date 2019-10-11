using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillSummon : MonoBehaviour
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [SerializeField]
    private Character summon;

    [FoldoutGroup("Special Behaviors", expanded: false)]
    [Tooltip("true = beim Start, ansonsten nach Delay")]
    [SerializeField]
    private bool summonInstantly = true;

    private void Start()
    {
        if (this.summonInstantly) summoning();
    }

    private void Update()
    {
        if (this.skill.delayTimeLeft <= 0 && !this.summonInstantly)
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
