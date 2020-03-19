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

    private new void Start()
    {
        base.Start();

        CustomUtilities.Format.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);
        this.childSprite.sprite = this.itemDrop.stats.getSprite();
        if (this.itemDrop == null) Destroy(this.gameObject);
    }

    public override void doSomethingOnSubmit()
    {
        if (this.player.GetComponent<PlayerUtils>().canOpenAndUpdateResource(this.price))
        {
            ItemStats loot = itemDrop.stats;

            this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.success, loot);
            if (loot != null) loot.CollectIt(this.player);
        }
        else
        {
            this.player.GetComponent<PlayerUtils>().showDialog(this, DialogTextTrigger.failed);
        }
    }
}
