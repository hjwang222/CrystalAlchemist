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
    public int dropRate = 100;

    [HideInInspector]
    public Loot lootEntry;

    [SerializeField]
    private Loot firstLoot;

    [SerializeField]
    private Loot alternativeLoot;

    public void setLoot()
    {
        this.lootEntry = this.firstLoot;
        if (this.lootEntry.item.stats.alreadyThere()) this.lootEntry = this.alternativeLoot;
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
            entry.setLoot();

            if(entry.dropRate > randomNumber)
            {
                ItemDrop item = Instantiate(entry.lootEntry.item);
                item.Initialize(entry.lootEntry.amount);
                loot.Add(item);
            }
        }

        return loot;
    }
}

