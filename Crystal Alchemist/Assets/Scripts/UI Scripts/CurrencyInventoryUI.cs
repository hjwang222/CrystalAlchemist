using UnityEngine;
using TMPro;

public class CurrencyInventoryUI : MonoBehaviour
{

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private ItemStats item;

    private void OnEnable()
    {
        Player player = this.playerStats.player;

        ItemStats item = player.GetComponent<PlayerUtils>().getItem(this.item);
        if (item != null) this.amountField.text = CustomUtilities.Format.formatString(item.amount, item.maxAmount);        
    }
}
