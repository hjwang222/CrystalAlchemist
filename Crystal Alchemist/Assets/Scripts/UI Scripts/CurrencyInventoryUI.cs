using UnityEngine;
using TMPro;

public class CurrencyInventoryUI : MonoBehaviour
{

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private ItemGroup item;

    private void OnEnable()
    {
        Player player = this.playerStats.player;
        this.amountField.text = player.GetComponent<PlayerItems>().GetAmountString(this.item);     
    }
}
