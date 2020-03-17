using Sirenix.OdinInspector;

[System.Serializable]
public class Loot
{
    [BoxGroup("Loot")]
    public Item item;

    [BoxGroup("Loot")]
    public int amount = 1;
}

[System.Serializable]
public class LootTable
{
    public int dropRate = 100;
    public Loot loot;
    public Loot alternativeLoot;
}
