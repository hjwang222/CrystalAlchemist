using UnityEngine;

public class PlayerItems : PlayerComponent
{
    [SerializeField]
    private PlayerInventory inventory;

    private void Start() => GameEvents.current.OnKeyItem += hasKeyItemAlready;

    private void OnDestroy() => GameEvents.current.OnKeyItem -= hasKeyItemAlready;

    public override void Initialize()
    {
        base.Initialize();
        this.inventory.Initialize(); //remove null objects          
    }

    public int GetAmount(ItemGroup group)
    {
        return this.inventory.GetAmount(group);
    }

    public void CollectInventoryItem(ItemStats item)
    {
        //Collect
        this.inventory.collectItem(item);
    }

    public void UpdateInventory(ItemGroup item, int amount)
    {
        this.inventory.UpdateInventory(item, amount);
    }

    public bool hasKeyItemAlready(string name)
    {
        foreach (ItemStats elem in this.inventory.keyItems) if (elem != null && name == elem.name) return true;      
        return false;
    }
}
