using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Mini Game Match")]
public class MiniGameMatch : ScriptableObject
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
        reward.setLoot();
    }
}
