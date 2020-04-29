using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private PlayerInventory inventory;

    private void Awake() => this.inventory.Initialize(); //remove null objects    

    public List<ItemStats> GetItemStats()
    {
        return this.inventory.keyItems;
    }

    public List<ItemGroup> GetItemGroups()
    {
        return this.inventory.inventoryItems;
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

    public void CollectInventoryItem(ItemGroup group, int amount)
    {
        //Save and Load System
        this.inventory.collectItem(group, amount);
    }

    public string GetAmountString(ItemGroup group)
    {
        return this.inventory.GetAmountString(group);        
    }

    public ItemStats getKeyItems(int ID)
    {
        return this.inventory.GetKeyItem(ID);
    }

    public ItemGroup getInventoryItems(int ID)
    {
        return this.inventory.GetInventoryItem(ID);
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
