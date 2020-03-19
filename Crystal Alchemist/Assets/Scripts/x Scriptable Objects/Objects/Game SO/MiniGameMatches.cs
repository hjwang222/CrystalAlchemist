using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[System.Serializable]
public class MiniGameMatch
{
    [FoldoutGroup("$difficulty")]
    [Range(1, 6)]
    public int maxRounds;

    [FoldoutGroup("$difficulty")]
    [Range(1, 6)]
    public int winsNeeded;

    [FoldoutGroup("$difficulty")]
    [Range(1, 5)]
    public int difficulty;

    [FoldoutGroup("$difficulty")]
    [Range(1, 120)]
    public float maxDuration;

    [FoldoutGroup("$difficulty")]
    public Price price;

    [FoldoutGroup("$difficulty")]
    public Reward reward;

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
