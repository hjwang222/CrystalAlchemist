using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ability/Sequence")]
public class SkillSequence : ScriptableObject
{
    [SerializeField]
    private List<BossMechanic> bossMechanics = new List<BossMechanic>();

    //Mode (Random, Together)
    //Delay/Timing

    public void InstantiateSequence(AI npc)
    {
        BossMechanic bossMechanic = this.bossMechanics[Random.Range(0, this.bossMechanics.Count)];

        BossMechanic newSequence = Instantiate(bossMechanic);
        newSequence.name = bossMechanic.name;
        newSequence.Initialize(npc, npc.target);
    }
}
