using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySlot : MonoBehaviour
{
    [SerializeField]
    private string itemGroup;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private TextMeshProUGUI textField;
    private Player player;


    void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        float value = Utilities.getAmountFromInventory(this.itemGroup, this.player.inventory, false);
        this.textField.text = Utilities.formatString(value, this.maxValue);
    }
}
