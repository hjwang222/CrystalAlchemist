using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField]
    public List<ItemStats> keyItems = new List<ItemStats>();

    [SerializeField]
    public List<ItemGroup> inventoryItems = new List<ItemGroup>();



    public void collectItem(ItemStats item)
    {
        if (item.isKeyItem())
        {
            this.keyItems.Add(Instantiate(item));
        }
        else
        {
            ItemGroup group = item.itemGroup;
            ItemGroup found = getItemGroup(group);

            if (found == null)
            {
                ItemGroup newGroup = Instantiate(group);
                newGroup.name = group.name;
                newGroup.UpdateAmount(item.getTotalAmount());
                this.inventoryItems.Add(newGroup);
            }
            else
            {
                found.UpdateAmount(item.getTotalAmount());
            }
        }
    }

    public void UpdateInventory(ItemGroup itemGroup, int value)
    {
        foreach (ItemGroup group in this.inventoryItems)
        {
            if (group == itemGroup)
            {
                group.UpdateAmount(value);
                break;
            }
        }
    }
       

    public ItemGroup getItemGroup(ItemGroup itemGroup)
    {
        foreach (ItemGroup group in this.inventoryItems)
        {
            if (group == itemGroup) return group;
        }
        return null;
    }

    public ItemGroup GetInventoryItem(int ID)
    {
        foreach (ItemGroup item in this.inventoryItems)
        {
            if (item.itemSlot == ID) return item;
        }

        return null;
    }

    public ItemGroup GetKeyItem(int ID)
    {
        foreach (ItemStats item in keyItems)
        {
            if (item.isID(ID)) return item.itemGroup;
        }

        return null;
    }

    public int GetAmount(ItemGroup itemGroup)
    {
        ItemGroup found = this.getItemGroup(itemGroup);
        if (found != null) return found.GetAmount();
        else return 0;
    }

    public string GetAmountString(ItemGroup itemGroup)
    {
        ItemGroup found = this.getItemGroup(itemGroup);
        if (found != null) return found.GetAmountString();
        else return "";
    }


    public bool hasKeyItemAlready(ItemStats item)
    {
        foreach (ItemStats elem in keyItems)
        {
            if (item.name == elem.name) return true;
        }

        return false;
    }
}
