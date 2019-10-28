using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemUI;

    [SerializeField]
    private int ID = 0;

    private void Awake()
    {
        if(this.ID <= 0) this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
    }

    public void openKeyItem(InventoryMenu menu)
    {
        if (this.itemUI.getItem() != null) menu.openSkillMenu(this.itemUI.getItem());
    }

    public void openMap(InventoryMenu menu)
    {
        if (this.itemUI.getItem() != null) menu.openMap();
    }

    public int getID()
    {
        return this.ID;
    }
    
    public void setItemToSlot(Item item)
    {
        this.itemUI.setItem(item);
    }
}
