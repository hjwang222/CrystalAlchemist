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

    private ItemStats item;

    public ItemStats getItem()
    {
        return this.item;
    }

    public void setItem(ItemStats item)
    {
        this.item = item;

        if (item == null)
        {
            this.image.gameObject.SetActive(false);
        }
        else
        {
            this.image.gameObject.SetActive(true);       

            if (!item.isKeyItem() && item.amount > 1) this.amount.text = "x" + item.amount;
            else if (this.amount != null) this.amount.text = "";

            if (this.preferInventoryIcon) this.image.sprite = item.getSprite();
            else this.image.sprite = item.getSprite();

            this.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
