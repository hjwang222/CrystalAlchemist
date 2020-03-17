using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[System.Serializable]
public class MiniGameMatch
{
    [Title("$difficulty", "", bold: true)]
    [BoxGroup("$difficulty", ShowLabel = false)]
    [Range(1, 6)]
    public int maxRounds;

    [BoxGroup("$difficulty")]
    [Range(1, 6)]
    public int winsNeeded;

    [BoxGroup("$difficulty")]
    [Range(1, 5)]
    public int difficulty;

    [BoxGroup("$difficulty")]
    [Range(1, 120)]
    public float maxDuration;

    [BoxGroup("Price")]
    public Price price;

    [BoxGroup("Loot")]
    public LootTableEntry reward;

    public void Initialize()
    {
        reward.Initialize();
    }
}

[CreateAssetMenu(menuName = "Game/Mini Game Matches")]
public class MiniGameMatches: ScriptableObject
{
    [SerializeField]
    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    public void Initialize()
    {
        foreach(MiniGameMatch match in this.matches)
        {
            match.Initialize();
        }
    }

    public List<MiniGameMatch> GetMatches()
    {
        return this.matches;
    }
}
