using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI amount;

    private Item item;
    private int ID;

    private void Awake()
    {
        this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
    }

    public int getID()
    {
        return this.ID;
    }

    public void setItemToSlot(Item item)
    {
        if (item == null)
            this.image.gameObject.SetActive(false);
        else
        {
            this.image.gameObject.SetActive(true);
            this.item = item;
            this.amount.text = "x"+item.amount;

            if (this.item.itemSpriteInventory != null)
            {
                this.image.sprite = this.item.itemSpriteInventory;
            }
            else
            {
                this.image.sprite = this.item.itemSprite;
            }
            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
