using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[System.Serializable]
public struct MiniGameMatch
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
    public Costs price;

    [FoldoutGroup("$difficulty")]
    [SerializeField]
    private Reward reward;

    private ItemDrop item;

    public void SetItem()
    {
        this.item = reward.GetItemDrop();
    }

    public ItemDrop getItem()
    {
        return this.item;
    }
}

[CreateAssetMenu(menuName = "Game/Menu/Mini Game Matches")]
public class MiniGameMatches: ScriptableObject
{
    [SerializeField]
    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    public void Initialize()
    {
        foreach(MiniGameMatch match in this.matches)
        {
            match.SetItem();
        }
    }

    public List<MiniGameMatch> GetMatches()
    {
        return this.matches;
    }
}
