using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

public class ShopItem : Rewardable
{
    [BoxGroup("Shop-Item Attribute")]
    [SerializeField]
    private SpriteRenderer childSprite;

    [BoxGroup("Loot")]
    [SerializeField]
    [HideLabel]
    private Reward reward;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private TextMeshPro priceText;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private Color fontColor;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private Color outlineColor;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private float outlineWidth = 0.25f;

    [BoxGroup("Easy Access")]
    [SerializeField]
    private Animator anim;

    private new void Start()
    {
        base.Start();
        this.setLoot();
        FormatUtil.set3DText(this.priceText, this.costs.amount + "", true, this.fontColor, this.outlineColor, this.outlineWidth);
        this.childSprite.sprite = this.itemDrop.stats.getSprite();
        if (this.itemDrop == null) Destroy(this.gameObject);
    }

    private void setLoot()
    {
        this.itemDrop = this.reward.GetItemDrop();
    }

    public override void doSomethingOnSubmit()
    {
        if (this.player.canUseIt(this.costs))
        {
            this.player.reduceResource(this.costs);
            ItemStats loot = itemDrop.stats;

            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.success, loot);
            if (loot != null) loot.CollectIt(this.player);
        }
        else
        {
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.failed);
        }
    }
}
