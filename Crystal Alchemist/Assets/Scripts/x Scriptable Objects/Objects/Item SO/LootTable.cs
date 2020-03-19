using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class Loot
{
    [HorizontalGroup(0.5f, MarginRight = 0.1f)]
    [HideLabel]
    public ItemDrop item;

    [MaxValue(99)]
    [MinValue(1)]
    [HorizontalGroup(0.1f)]
    [LabelWidth(60)]
    [ShowIf("item")]
    public int amount = 1;
}

[System.Serializable]
public class LootTableEntry
{
    [SerializeField]
    [HideLabel]
    private Reward reward;

    [HorizontalGroup(0.5f, MarginRight = 0.1f)]
    public bool hasDropRate = false;

    [ShowIf("hasDropRate")]
    [HorizontalGroup(0.1f)]
    [LabelWidth(60)]
    [MaxValue(100)]
    [MinValue(1)]
    public int dropRate = 100;


    public void Initialize()
    {
        this.reward.Initialize();
    }

    public Loot getLoot()
    {
        return this.reward.getLoot();
    }
}

[System.Serializable]
public class Reward
{    
    [SerializeField]
    [HideLabel]    
    private Loot firstLoot;

    [SerializeField]
    [HorizontalGroup(0.5f, MarginRight = 0.1f)]
    private bool hasAlternative = false;

    [ShowIf("hasAlternative")]
    [SerializeField]
    [HideLabel]
    private Loot alternativeLoot;

    private Loot loot;

    public void Initialize()
    {
        if (this.hasAlternative && this.firstLoot.item.stats.alreadyThere()) this.loot = this.alternativeLoot;
        else this.loot = this.firstLoot;
    }

    public Loot getLoot()
    {
        return this.loot;
    }
}

[CreateAssetMenu(menuName = "Game/Items/Loot Table")]
public class LootTable : ScriptableObject
{
    [SerializeField]
    private List<LootTableEntry> entries = new List<LootTableEntry>();

    public ItemDrop SetLoot()
    {
        int randomNumber = Random.Range(1, 100);
        List<Loot> loot = new List<Loot>();
        ItemDrop item = null;

        foreach (LootTableEntry entry in this.entries)
        {
            entry.Initialize();
            Loot lootItem = entry.getLoot();

            if (entry.dropRate > randomNumber || !entry.hasDropRate) loot.Add(lootItem);
        }

        if (loot.Count > 0)
        {
            Loot result = loot[Random.Range(0, loot.Count)];

            item = Instantiate(result.item);
            item.Initialize(result.amount);
        }

        return item;
    }
}
