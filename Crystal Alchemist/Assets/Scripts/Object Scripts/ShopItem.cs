using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : Rewardable
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

    private new void Start()
    {
        base.Start();

        CustomUtilities.Format.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);

        this.inventory.Add(this.lootTableInternal[this.index].item);

        //this.amountText.text = this.amount + "";
        this.childSprite.sprite = this.inventory[this.index].itemSprite;

        //TODO Item:
        if (this.inventory.Count == 0) this.gameObject.SetActive(false);
    }

    public override void doSomethingOnSubmit()
    {
        if (CustomUtilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price))
        {
            Item loot = inventory[this.index];

            CustomUtilities.DialogBox.showDialog(this, this.player, DialogTextTrigger.success, loot);
            this.player.collect(loot, false);
        }
        else
        {
            CustomUtilities.DialogBox.showDialog(this, this.player, DialogTextTrigger.failed);
        }
    }
}
