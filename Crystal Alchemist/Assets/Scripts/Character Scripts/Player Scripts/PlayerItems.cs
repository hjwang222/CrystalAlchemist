using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private PlayerInventory inventory;

    private void Start()
    {
        this.inventory.Initialize(); //remove null objects  
        this.player = this.GetComponent<Player>();
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
