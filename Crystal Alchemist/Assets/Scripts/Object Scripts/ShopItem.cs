using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : Interactable
{

    [Header("Shop-Item Attribute")]
    public SpriteRenderer childSprite;

    [Header("Text-Attribute")]
    public TextMeshPro priceText;
    public Color fontColor;
    public Color outlineColor;
    public float outlineWidth = 0.25f;
    public Animator anim;

    private int index = 0;
    //private int amount = 1;
    
    private void Start()
    {
        init();

        Utilities.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);

        this.items.Add(this.lootTable[this.index].item);

        //this.amountText.text = this.amount + "";
        this.childSprite.sprite = this.items[this.index].getSprite();
    }

    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("Submit"))
        {
            if (Utilities.canOpen(this.currencyNeeded, this.item, this.player, this.price))
            {
                this.player.showDialogBox("Du hast 1 " + this.items[this.index].itemName + " für " + this.price + " gekauft!");
                this.player.collect(items[this.index], false);
            }
        }
    }
}
