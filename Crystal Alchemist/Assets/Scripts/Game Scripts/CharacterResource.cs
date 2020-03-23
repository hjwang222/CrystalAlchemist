using Sirenix.OdinInspector;
using UnityEngine;

public enum ResourceType
{
    none,
    life,
    mana,
    item
}

[System.Serializable]
public class BaseResource
{
    public ResourceType resourceType = ResourceType.none;

    [ShowIf("resourceType", ResourceType.item)]
    public ItemGroup item;
}

[System.Serializable]
public class CharacterResource : BaseResource
{
    [HideIf("resourceType", ResourceType.none)]
    public float amount = 1;
}

[System.Serializable]
public class Costs : BaseResource
{
    [HideIf("resourceType", ResourceType.none)]
    [MinValue(0)]
    public float amount = 1;
}