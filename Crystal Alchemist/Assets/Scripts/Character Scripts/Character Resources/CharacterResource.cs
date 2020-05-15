using Sirenix.OdinInspector;
using UnityEngine;

public enum CostType
{
    none,
    life,
    mana,
    item
}

[System.Serializable]
public class BaseResource
{
    public CostType resourceType = CostType.none;

    [ShowIf("resourceType", CostType.item)]
    public ItemGroup item;
}

[System.Serializable]
public class CharacterResource : BaseResource
{
    [HideIf("resourceType", CostType.none)]
    public float amount = 1;
}

[System.Serializable]
public class Costs : BaseResource
{
    [HideIf("resourceType", CostType.none)]
    [MinValue(0)]
    public float amount = 1;
}