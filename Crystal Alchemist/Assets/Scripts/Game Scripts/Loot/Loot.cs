using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public ItemDrop item;
    public int amount = 1;
}

[System.Serializable]
public class LootTableEntry
{
    public bool hasDropRate = false;

    [ShowIf("hasDropRate")]
    public int dropRate = 100;

    [SerializeField]
    [BoxGroup("First")]
    private Loot firstLoot;

    [SerializeField]
    [BoxGroup("Second")]
    private bool hasAlternative = false;

    [ShowIf("hasAlternative")]
    [SerializeField]
    [BoxGroup("Second")]
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
    public List<LootTableEntry> entries = new List<LootTableEntry>();

    public List<ItemDrop> SetLoot()
    {
        int randomNumber = Random.Range(1, 100);
        List<ItemDrop> loot = new List<ItemDrop>();

        foreach (LootTableEntry entry in this.entries)
        {
            entry.Initialize();
            Loot lootItem = entry.getLoot();

            if(entry.dropRate > randomNumber || !entry.hasDropRate)
            {
                ItemDrop item = Instantiate(lootItem.item);
                item.Initialize(lootItem.amount);
                loot.Add(item);
            }
        }

        return loot;
    }
}

