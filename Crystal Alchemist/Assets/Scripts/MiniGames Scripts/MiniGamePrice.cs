using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGamePrice : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI textField;

    public bool updatePrice(Player player, Costs price)
    {
        this.image.enabled = false;

        if (price.item != null && price.resourceType == ResourceType.item)
        {
            this.image.enabled = true;
            int inventoryAmount = player.GetComponent<PlayerItems>().GetAmount(price.item);
            this.image.sprite = price.item.getSprite();
            this.textField.text = price.amount + " / " + inventoryAmount;
        }
        else if (price.resourceType == ResourceType.none)
        {
            this.textField.text = FormatUtil.getLanguageDialogText("GRATIS", "FREE");
        }

        bool canStart = player.HasEnoughCurrency(price);
        setColor(canStart);

        return canStart;
    }

    private void setColor(bool canStart)
    {
        if (canStart) this.textField.color = Color.white;
        else this.textField.color = Color.red;
    }
}
