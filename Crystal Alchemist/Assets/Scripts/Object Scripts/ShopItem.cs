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
        this.childSprite.sprite = this.inventory[this.index].stats.getSprite();
        if (this.inventory.Count == 0) Destroy(this.gameObject);
    }

    public override void doSomethingOnSubmit()
    {
        if (this.player.GetComponent<PlayerUtils>().canOpenAndUpdateResource(this.price))
        {
            ItemStats loot = inventory[this.index].stats;

            this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.success, loot);
            this.player.GetComponent<PlayerUtils>().CollectItem(loot);
        }
        else
        {
            this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.failed);
        }
    }
}
