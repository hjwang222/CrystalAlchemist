using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : Interactable
{

    [Header("Shop-Item Attribute")]
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI amountText;
    public SpriteRenderer childSprite;

    private int index = 0;
    private int amount = 1;
    
    private void Start()
    {
        init();

        this.priceText.text = price + "";

        this.items.Add(this.lootTable[this.index].item);

        //this.amountText.text = this.amount + "";
        this.childSprite.sprite = this.items[this.index].getSprite();
    }

    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("Submit"))
        {
            if (Utilities.canOpen(this.currencyNeeded, this.player, this.price))
            {
                this.player.showDialogBox("Du hast 1 " + this.items[this.index].itemName + " für " + this.price + " gekauft!");
                items[this.index].collectByPlayer(this.player, false);
            }
        }
    }
}
