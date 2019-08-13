using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyInventoryUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI amountField;

    [SerializeField]
    private Item item;

    private void OnEnable()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

        Item item = Utilities.Items.getItemFromInventory(this.item, player.inventory);

        string text = "";
        if (item != null) text = Utilities.Format.formatString(item.amount, item.maxAmount);

        this.amountField.text = text;
    }
}
