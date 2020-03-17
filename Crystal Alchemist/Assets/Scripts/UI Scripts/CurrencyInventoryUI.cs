using UnityEngine;
using TMPro;

public class CurrencyInventoryUI : MonoBehaviour
{

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private InventoryItem item;

    private void OnEnable()
    {
        Player player = this.playerStats.player;

        InventoryItem item = CustomUtilities.Items.getItemFromInventory(this.item, player.inventory);
        if (item != null) this.amountField.text = CustomUtilities.Format.formatString(item.amount, item.maxAmount);        
    }
}
