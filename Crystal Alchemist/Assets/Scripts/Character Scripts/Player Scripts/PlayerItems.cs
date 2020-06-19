using UnityEngine;

public class PlayerItems : PlayerComponent
{
    [SerializeField]
    private PlayerInventory inventory;

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

    public bool hasKeyItemAlready(ItemStats item)
    {
        return this.inventory.hasKeyItemAlready(item);
    } 
}
