using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGamePrice : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI textField;

    public bool updatePrice(Player player, Price price)
    {
        this.image.enabled = false;

        if (price.item != null && price.resourceType == ResourceType.item)
        {
            this.image.enabled = true;
            int inventoryAmount = player.GetComponent<PlayerUtils>().getItemAmount(price.item);
            this.image.sprite = price.item.getSprite();
            this.textField.text = price.amount + " / " + inventoryAmount;
        }
        else if (price.resourceType == ResourceType.none)
        {
            this.textField.text = CustomUtilities.Format.getLanguageDialogText("GRATIS", "FREE");
        }

        bool canStart = player.GetComponent<PlayerUtils>().hasEnoughCurrency(price);
        setColor(canStart);

        return canStart;
    }

    private void setColor(bool canStart)
    {
        if (canStart) this.textField.color = Color.white;
        else this.textField.color = Color.red;
    }
}
