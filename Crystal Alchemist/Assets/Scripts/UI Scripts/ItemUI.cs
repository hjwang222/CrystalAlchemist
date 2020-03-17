using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI amount;

    [SerializeField]
    private bool preferInventoryIcon = true;

    private Item item;

    public Item getItem()
    {
        return this.item;
    }

    public void setItem(Item item)
    {
        this.item = item;
        setItem(item.GetInventoryItem());
    }

    public void setItem(InventoryItem item)
    {
        if (item == null)
        {
            this.image.gameObject.SetActive(false);
        }
        else
        {
            this.image.gameObject.SetActive(true);       

            if (!item.isKeyItem && item.amount > 1) this.amount.text = "x" + item.amount;
            else if (this.amount != null) this.amount.text = "";

            if(this.preferInventoryIcon) CustomUtilities.Items.setItemImage(this.image, item);
            else this.image.sprite = item.itemSprite;

            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
