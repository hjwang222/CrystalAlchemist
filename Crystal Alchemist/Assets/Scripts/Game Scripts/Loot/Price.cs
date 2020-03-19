using Sirenix.OdinInspector;

public enum ResourceType
{
    none,
    life,
    mana,
    item
}

[System.Serializable]
public class Price
{
    public ResourceType resourceType = ResourceType.item;

    [ShowIf("resourceType", ResourceType.item)]
    public ItemGroup item;

    [HideIf("resourceType", ResourceType.none)]
    public float amount = 1;
}
