using Sirenix.OdinInspector;

public enum ResourceType
{
    none,
    life,
    mana,
    item,
    skill,
    statuseffect
}

[System.Serializable]
public class Price
{
    [BoxGroup("Price")]
    public ResourceType resourceType = ResourceType.item;

    [BoxGroup("Price")]
    [ShowIf("currencyType", ResourceType.item)]
    public InventoryItem item;

    [BoxGroup("Price")]
    public float amount = 1;
}
