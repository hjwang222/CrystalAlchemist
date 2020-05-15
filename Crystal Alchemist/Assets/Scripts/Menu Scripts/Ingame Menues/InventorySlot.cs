﻿using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemUI;

    [SerializeField]
    private int ID = 0;

    public int Initialize()
    {
        if (this.ID <= 0) this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
        return this.ID;
    }

    public void openKeyItem(InventoryMenu menu)
    {
        this.itemUI.getItemStat().inventoryInfo.raiseKeySignal();
        menu.ExitMenu();
    }

    public void setItemToSlot(ItemGroup item)
    {
        this.itemUI.setItem(item);
    }

    public void setItemToSlot(ItemStats item)
    {
        this.itemUI.setItem(item);
    }
}
