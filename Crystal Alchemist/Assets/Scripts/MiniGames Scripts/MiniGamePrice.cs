using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGamePrice : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI textField;

    public bool updatePrice(PlayerInventory inventory, Costs price)
    {
        this.image.enabled = false;

        if (price.item != null && price.resourceType == CostType.item)
        {
            this.image.enabled = true;
            int inventoryAmount = inventory.GetAmount(price.item);
            this.image.sprite = price.item.info.getSprite();
            this.textField.text = price.amount + " / " + inventoryAmount;
        }
        else if (price.resourceType == CostType.none)
        {
            this.textField.text = "";
        }

        bool canStart = inventory.HasEnoughCurrency(price);
        setColor(canStart);

        return canStart;
    }

    private void setColor(bool canStart)
    {
        if (canStart) this.textField.color = Color.white;
        else this.textField.color = Color.red;
    }
}
