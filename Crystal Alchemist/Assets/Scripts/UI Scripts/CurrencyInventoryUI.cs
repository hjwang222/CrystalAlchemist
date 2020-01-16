using UnityEngine;
using TMPro;

public class CurrencyInventoryUI : MonoBehaviour
{

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private Item item;

    private void OnEnable()
    {
        Player player = this.playerStats.player;

        Item item = CustomUtilities.Items.getItemFromInventory(this.item, player.inventory);
        if (item != null) this.amountField.text = CustomUtilities.Format.formatString(item.amount, item.maxAmount);        
    }
}
