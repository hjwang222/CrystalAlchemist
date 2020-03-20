using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil : MonoBehaviour
{
    public static Ability InstantiateAbility(Ability ability)
    {
        return Instantiate(ability);
    }

    public static SkillSequence instantiateSequence(SkillSequence skillSequence, AI npc, Vector2 sequencePosition, modificationType type)
    {
        SkillSequence sequence = Instantiate(skillSequence);

        sequence.setSender(npc);
        sequence.setTarget(npc.target);
        sequence.setPosition(type, sequencePosition);
        return sequence;
    }

    public static Skill instantiateSkill(Skill skill, Character sender)
    {
        return instantiateSkill(skill, sender, null, 1);
    }

    public static Skill instantiateSkill(Skill skill, Character sender, Character target)
    {
        return instantiateSkill(skill, sender, target, 1);
    }

    public static Skill instantiateSkill(Skill skill, Character sender, Character target, float reduce)
    {
        if (skill != null
            && sender.currentState != CharacterState.attack
            && sender.currentState != CharacterState.defend)
        {
            Skill activeSkill = Instantiate(skill, sender.transform.position, Quaternion.identity);
            SkillTargetModule targetModule = activeSkill.GetComponent<SkillTargetModule>();
            SkillSenderModule sendermodule = activeSkill.GetComponent<SkillSenderModule>();

            if (skill.attachToSender) activeSkill.transform.parent = sender.activeSkillParent.transform;

            if (target != null) activeSkill.target = target;
            activeSkill.sender = sender;

            List<Price> temp = new List<Price>();

            if (targetModule != null)
            {
                for (int i = 0; i < targetModule.affectedResources.Count; i++)
                {
                    Price elem = targetModule.affectedResources[i];
                    elem.amount /= reduce;
                    temp.Add(elem);
                }

                targetModule.affectedResources = temp;
                if (sendermodule != null) sendermodule.price.amount /= reduce;
            }

            sender.activeSkills.Add(activeSkill);
            return activeSkill.GetComponent<Skill>();
        }

        return null;
    }

    public static Skill getSkillByCollision(GameObject collision)
    {
        return collision.GetComponentInParent<Skill>();
    }
}
