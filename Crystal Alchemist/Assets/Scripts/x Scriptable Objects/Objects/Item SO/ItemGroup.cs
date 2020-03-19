using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Items/Item Group")]
public class ItemGroup : ScriptableObject
{
    [BoxGroup("Texts")]
    [SerializeField]
    private string itemGroup;

    [BoxGroup("Texts")]
    [SerializeField]
    private string itemGroupEnglish;

    [Space(10)]
    [BoxGroup("Texts")]
    [TextArea]
    [SerializeField]
    [ShowIf("inventoryItem")]
    private string description;

    [BoxGroup("Texts")]
    [TextArea]
    [SerializeField]
    [ShowIf("inventoryItem")]
    private string descriptionEnglish;

    [BoxGroup("Inventory")]
    [SerializeField]
    public bool inventoryItem = false;

    [BoxGroup("Inventory")]
    public int maxAmount;

    [BoxGroup("Inventory")]
    [SerializeField]
    [ShowIf("inventoryItem")]
    [AssetIcon]
    private Sprite inventorySprite;

    [BoxGroup("Inventory")]
    [SerializeField]
    [ShowIf("inventoryItem")]
    public int itemSlot = -1;

    [BoxGroup("Inventory")]
    [SerializeField]
    [ShowIf("inventoryItem")]
    public bool isKeyItem = false;

    [BoxGroup("Signals")]
    [ShowIf("isKeyItem")]
    [SerializeField]
    private SimpleSignal keyItemSignal;

    [BoxGroup("Signals")]
    [SerializeField]
    private SimpleSignal collectSignal;

    private int amount;

    public Sprite getSprite()
    {
        return this.inventorySprite;
    }

    public string getName()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemGroup, this.itemGroupEnglish);
    }

    public string getDescription()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.description, this.descriptionEnglish);
    }

    public int GetAmount()
    {
        return this.amount;
    }

    public string GetAmountString()
    {
        return CustomUtilities.Format.formatString(this.amount, this.maxAmount);
    }

    public void UpdateAmount(int amount)
    {
        this.amount += amount;
    }

    public void raiseKeySignal()
    {
        if(this.keyItemSignal != null) this.keyItemSignal.Raise();
    }

    public void raiseCollectSignal()
    {
        if (this.collectSignal != null) this.collectSignal.Raise();
    }

    public string getItemGroup()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemGroup, this.itemGroupEnglish);
    }
}
