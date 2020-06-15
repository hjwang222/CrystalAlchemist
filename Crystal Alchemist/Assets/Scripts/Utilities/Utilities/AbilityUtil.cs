using System.Collections.Generic;
using UnityEngine;

public static class AbilityUtil
{
    public static Ability InstantiateAbility(Ability ability)
    {
        Ability newAbility = MonoBehaviour.Instantiate(ability);
        newAbility.Initialize();
        newAbility.name = ability.name;
        return newAbility;
    }

    public static void instantiateSequence(BossMechanic sequence, AI npc, List<string> patterns)
    {
        BossMechanic newSequence = MonoBehaviour.Instantiate(sequence);
        newSequence.name = sequence.name;
        newSequence.Initialize(npc, npc.target);
    }    

    public static Skill getSkillByCollision(GameObject collision)
    {
        return collision.GetComponentInParent<Skill>();
    }
}
