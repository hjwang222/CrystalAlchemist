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

    private new void Start()
    {
        base.Start();

        Utilities.Format.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);

        this.items.Add(this.lootTable[this.index].item);

        //this.amountText.text = this.amount + "";
        this.childSprite.sprite = this.items[this.index].itemSprite;
    }

    public override void doSomethingOnSubmit()
    {
        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);

        if (Utilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price, text))
        {
            Item loot = items[this.index];

            string itemObtained = Utilities.Format.getDialogBoxText("Du hast", loot.amount, loot, "für");
            string itemNedded = Utilities.Format.getDialogBoxText("", this.price, this.item, "gekauft!");

            if (GlobalValues.useAlternativeLanguage)
            {
                itemObtained = Utilities.Format.getDialogBoxText("You bought", loot.amount, loot, "for");
                itemNedded = Utilities.Format.getDialogBoxText("", this.price, this.item, "!");
            }

            this.player.showDialogBox(itemObtained + "\n" + itemNedded);
            this.player.collect(loot, false);
        }
    }
}
