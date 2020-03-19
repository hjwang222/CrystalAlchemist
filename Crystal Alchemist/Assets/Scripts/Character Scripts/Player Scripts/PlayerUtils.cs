using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private PlayerInventory inventory;

    public List<ItemStats> GetItemStats()
    {
        return this.inventory.keyItems;
    }

    public int GetAmount(ItemGroup group)
    {
        return this.inventory.GetAmount(group);
    }

    public void CollectInventoryItem(ItemStats item)
    {
        this.inventory.collectItem(item);
    }

    public string GetAmountString(ItemGroup group)
    {
        return this.inventory.GetAmountString(group);        
    }

    public ItemGroup getKeyItems(int ID)
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
        item.raiseCollectSignal();
    }

    public bool hasEnoughCurrency(Price price)
    {
        bool result = false;

        if (price.resourceType == ResourceType.none) result = true;
        else 
        {
            if (this.player.getResource(price.resourceType, price.item) - price.amount >= 0) result = true;
            else result = false;
        }

        return result;
    }

    public bool hasKeyItemAlready(ItemStats item)
    {
        return this.inventory.hasKeyItemAlready(item);
    }

    public bool canOpenAndUpdateResource(Price price)
    {
        if (this.player.currentState != CharacterState.inDialog
            && this.player.currentState != CharacterState.respawning
            && this.player.currentState != CharacterState.inMenu)
        {
            if (hasEnoughCurrency(price))
            {
                reduceCurrency(price);
                return true;
            }
        }

        return false;
    }

    public void reduceCurrency(Price price)
    {
        if ((price.item != null && !price.item.isKeyItem) || price.item == null)
            this.player.updateResource(price.resourceType, price.item, -price.amount);
    }


    public void showDialog(Interactable interactable)
    {
        showDialog(interactable, null);
    }

    public void showDialog(Interactable interactable, DialogTextTrigger trigger)
    {
        showDialog(interactable, trigger, null);
    }

    public void showDialog(Interactable interactable, ItemStats loot)
    {
        if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.GetComponent<DialogSystem>().show(this.player, interactable, loot);
    }

    public void showDialog(Interactable interactable, DialogTextTrigger trigger, ItemStats loot)
    {
        if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.gameObject.GetComponent<DialogSystem>().show(this.player, trigger, interactable, loot);
    }
}
