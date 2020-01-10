using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGamePrice : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI textField;

    public void updatePrice(Item item, int amount, Player player)
    {
        int inventory = CustomUtilities.Items.getAmountFromInventory(item, player.inventory, false);
        CustomUtilities.Items.setItemImage(this.image, item);
        this.textField.text = amount + " / " + inventory; 
    }

    public void setColor(bool canStart)
    {
        if (canStart) this.textField.color = Color.white;
        else this.textField.color = Color.red;
    }
}
