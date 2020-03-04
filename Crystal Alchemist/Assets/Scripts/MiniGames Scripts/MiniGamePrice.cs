using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGamePrice : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI textField;

    public bool updatePrice(ResourceType currencyType, Item item, int amount, Player player)
    {
        this.image.enabled = false;

        if (item != null && currencyType == ResourceType.item)
        {
            this.image.enabled = true;
            int inventory = CustomUtilities.Items.getAmountFromInventory(item, player.inventory);
            CustomUtilities.Items.setItemImage(this.image, item);
            this.textField.text = amount + " / " + inventory;
        }
        else if (currencyType == ResourceType.none)
        {
            this.textField.text = CustomUtilities.Format.getLanguageDialogText("GRATIS", "FREE");
        }

        bool canStart = CustomUtilities.Items.hasEnoughCurrency(currencyType, player, item, amount);
        setColor(canStart);

        return canStart;
    }

    private void setColor(bool canStart)
    {
        if (canStart) this.textField.color = Color.white;
        else this.textField.color = Color.red;
    }
}
