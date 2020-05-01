using UnityEngine;

public class AbilityUtil : MonoBehaviour
{
    public static Ability InstantiateAbility(Ability ability)
    {
        Ability newAbility = Instantiate(ability);
        newAbility.Initialize();
        newAbility.name = ability.name;
        return newAbility;
    }

    public static void instantiateSequence(BossMechanic sequence, AI npc)
    {
        BossMechanic newSequence = Instantiate(sequence);
        newSequence.name = sequence.name;
        newSequence.Initialize(npc, npc.target);
    }    

    public static Skill getSkillByCollision(GameObject collision)
    {
        return collision.GetComponentInParent<Skill>();
    }
}
