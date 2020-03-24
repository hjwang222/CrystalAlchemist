using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Items/Item Group")]
public class ItemGroup : ScriptableObject
{
    [BoxGroup("Inventory")]
    public int maxAmount;

    [BoxGroup("Inventory")]
    [SerializeField]
    [Required]
    public ItemInfo info;

    [BoxGroup("Inventory")]
    [SerializeField]
    [Required]
    public ItemSlotInfo inventoryInfo;

    [BoxGroup("Signals")]
    [SerializeField]
    private SimpleSignal collectSignal;

    private int amount;

    [AssetIcon]
    public Sprite GetSprite()
    {
        if (this.info != null) return this.info.getSprite();
        return null;
    }

    public bool isKeyItem()
    {
        if (this.inventoryInfo != null) return this.inventoryInfo.isKeyItem();
        else return false;
    }

    public bool isID(int ID)
    {
        if (this.inventoryInfo != null) return this.inventoryInfo.isID(ID);
        return false;
    }

    public string getName()
    {
        return this.info.getName();
    }

    public int GetAmount()
    {
        return this.amount;
    }

    public string GetAmountString()
    {
        return FormatUtil.formatString(this.amount, this.maxAmount);
    }

    public void UpdateAmount(int amount)
    {
        this.amount += amount;
    }

    public void raiseCollectSignal()
    {
        if (this.collectSignal != null) this.collectSignal.Raise();
    }
}
