using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField]
    public List<ItemStats> inventory = new List<ItemStats>();       

    public void UpdateInventory(ItemStats item, int amount)
    {
        ItemStats found = GetItem(item);

        if (found == null)
        {
            ItemStats newItem = Instantiate(item);
            newItem.name = item.name;
            newItem.amount = amount;
            this.inventory.Add(newItem);
        }
        else
        {
            if (!item.isKeyItem())
            {
                found.amount += amount;
                if (found.amount <= 0) this.inventory.Remove(found);
            }
        }
    }

    public ItemStats GetItem(ItemStats item)
    {
        foreach (ItemStats elem in inventory)
        {
            if (item.isKeyItem())
            {
                if (item.name == elem.name) return elem;
            }
            else
            {
                if (item.itemGroup == elem.itemGroup) return elem;
            }
        }
        return null;
    }

    public ItemStats GetItem(int ID, bool isKeyItem)
    {
        foreach (ItemStats item in inventory)
        {
            return item.getInventoryItem(ID, isKeyItem);
        }

        return null;
    }

    public bool hasKeyItemAlready(ItemStats item)
    {
        if (item.isKeyItem())
        {
            foreach (ItemStats elem in inventory)
            {
                if (item.name == elem.name) return true;
            }
        }
        return false;
    }

    private void OnDisable()
    {
        this.inventory.Clear();
    }
}
