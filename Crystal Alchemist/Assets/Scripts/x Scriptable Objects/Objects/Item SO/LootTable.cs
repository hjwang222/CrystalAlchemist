using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class Loot
{
    [HorizontalGroup("Item")]
    public ItemDrop item;

    [HorizontalGroup("Item")]
    public int amount = 1;
}

[System.Serializable]
public class LootTableEntry
{
    [HorizontalGroup("Droprate")]
    public bool hasDropRate = false;

    [HorizontalGroup("Droprate")]
    [ShowIf("hasDropRate")]
    public int dropRate = 100;

    [SerializeField]
    private Reward reward;

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
    private Loot firstLoot;

    [SerializeField]
    private bool hasAlternative = false;

    [ShowIf("hasAlternative")]
    [SerializeField]
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

    public List<ItemDrop> SetLoot()
    {
        int randomNumber = Random.Range(1, 100);
        List<ItemDrop> loot = new List<ItemDrop>();

        foreach (LootTableEntry entry in this.entries)
        {
            entry.Initialize();
            Loot lootItem = entry.getLoot();

            if (entry.dropRate > randomNumber || !entry.hasDropRate)
            {
                ItemDrop item = Instantiate(lootItem.item);
                item.Initialize(lootItem.amount);
                loot.Add(item);
            }
        }

        return loot;
    }
}
