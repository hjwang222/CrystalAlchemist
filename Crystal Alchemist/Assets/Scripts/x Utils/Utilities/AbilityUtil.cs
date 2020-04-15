using System.Collections.Generic;
using UnityEngine;


public class AbilityUtil : MonoBehaviour
{
    public static Ability InstantiateAbility(Ability ability)
    {
        return Instantiate(ability);
    }

    public static void instantiateSequence(BossMechanic sequence, AI npc)
    {
        BossMechanic newSequence = Instantiate(sequence);
        newSequence.name = sequence.name;
        newSequence.Initialize(npc, npc.target);
    }

    public static void instantiateSkill(Ability ability, Character sender, Character target)
    {
        //Single Target
        instantiateSkill(ability, sender, target, 1);
    }

    public static void instantiateSkill(Ability ability, Character sender, Character target, float reduce)
    {
        if (ability.skill != null && sender != null)
        {
            Skill activeSkill = Instantiate(ability.skill, sender.transform.position, Quaternion.identity);
            activeSkill.name = ability.skill.name;
            activeSkill.Initialize(ability.positionOffset, ability.lockDirection, ability.timeDistortion, ability.attachToSender);
            activeSkill.SetMaxDuration(ability.hasMaxDuration, ability.maxDuration);

            if (ability.attachToSender) activeSkill.transform.parent = sender.activeSkillParent.transform;
            if (target != null) activeSkill.target = target;
            activeSkill.sender = sender;

            ReduceCostAndDamage(activeSkill, reduce, ability.shareDamage);
            sender.activeSkills.Add(activeSkill);
        }
    }

    private static void ReduceCostAndDamage(Skill activeSkill, float reduce, bool shareDamage)
    {
        SkillTargetModule targetModule = activeSkill.GetComponent<SkillTargetModule>();
        SkillSenderModule sendermodule = activeSkill.GetComponent<SkillSenderModule>();        

        if (targetModule != null && shareDamage)
        {
            List<CharacterResource> temp = new List<CharacterResource>();

            for (int i = 0; i < targetModule.affectedResources.Count; i++)
            {
                CharacterResource elem = targetModule.affectedResources[i];
                elem.amount /= reduce;
                temp.Add(elem);
            }

            targetModule.affectedResources = temp;            
        }

        if (sendermodule != null) sendermodule.costs.amount /= reduce;
    }

    public static Skill getSkillByCollision(GameObject collision)
    {
        return collision.GetComponentInParent<Skill>();
    }
}
