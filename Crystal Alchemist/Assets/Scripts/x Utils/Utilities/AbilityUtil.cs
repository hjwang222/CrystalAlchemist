using System.Collections.Generic;
using UnityEngine;


public class AbilityUtil : MonoBehaviour
{
    public static Ability InstantiateAbility(Ability ability)
    {
        return Instantiate(ability);
    }

    public static void instantiateSequence(BossSequence sequence, AI npc)
    {
        BossSequence newSequence = Instantiate(sequence);
        newSequence.name = sequence.name;
        newSequence.Initialize(npc, npc.target);
    }

    public static void instantiateSkill(Skill skill, Character sender)
    {
        //Player Call
        instantiateSkill(skill, sender, null, 1);
    }

    public static void instantiateSkill(Skill skill, Character sender, Character target)
    {
        //AI Call
        instantiateSkill(skill, sender, target, 1);
    }

    public static void instantiateSkill(Skill skill, Character sender, Character target, float reduce)
    {
        if (skill != null)
        {
            Skill activeSkill = Instantiate(skill, sender.transform.position, Quaternion.identity);
            activeSkill.name = skill.name;
            if (skill.attachToSender) activeSkill.transform.parent = sender.activeSkillParent.transform;
            if (target != null) activeSkill.target = target;
            activeSkill.sender = sender;

            ReduceCostAndDamage(activeSkill, reduce);

            sender.activeSkills.Add(activeSkill);
        }
    }

    private static void ReduceCostAndDamage(Skill activeSkill, float reduce)
    {
        SkillTargetModule targetModule = activeSkill.GetComponent<SkillTargetModule>();
        SkillSenderModule sendermodule = activeSkill.GetComponent<SkillSenderModule>();
        List<CharacterResource> temp = new List<CharacterResource>();

        if (targetModule != null)
        {
            for (int i = 0; i < targetModule.affectedResources.Count; i++)
            {
                CharacterResource elem = targetModule.affectedResources[i];
                elem.amount /= reduce;
                temp.Add(elem);
            }

            targetModule.affectedResources = temp;
            if (sendermodule != null) sendermodule.costs.amount /= reduce;
        }
    }

    public static Skill getSkillByCollision(GameObject collision)
    {
        return collision.GetComponentInParent<Skill>();
    }
}
