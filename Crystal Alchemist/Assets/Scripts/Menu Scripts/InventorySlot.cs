using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Image image;
    public Item item;
    public int ID;

    private void Awake()
    {
        this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
    }

    public void setItemToSlot(Item item)
    {
        if (item == null)
            this.image.enabled = false;
        else
        {
            this.item = item;
            this.image.enabled = true;
            this.image.sprite = this.item.getSprite();
            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
