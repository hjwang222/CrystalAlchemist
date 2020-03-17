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
    public ResourceType resourceType = ResourceType.item;

    [ShowIf("resourceType", ResourceType.item)]
    public ItemStats item;

    [HideIf("resourceType", ResourceType.none)]
    public float amount = 1;
}
