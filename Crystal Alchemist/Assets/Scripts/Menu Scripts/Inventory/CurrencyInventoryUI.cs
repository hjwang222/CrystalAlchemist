using UnityEngine;
using TMPro;

public class CurrencyInventoryUI : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory inventory;

    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private ItemGroup item;

    private void OnEnable()
    {
        this.amountField.text = this.inventory.GetAmountString(this.item);     
    }
}
