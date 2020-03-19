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

    [BoxGroup("Inventory")]
    [ShowIf("isKeyItem")]
    [SerializeField]
    private SimpleSignal keyItemSignal;

    public Sprite getSprite()
    {
        return this.inventorySprite;
    }

    public void raiseKeySignal()
    {
        if(this.keyItemSignal != null) this.keyItemSignal.Raise();
    }

    public string getItemGroup()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemGroup, this.itemGroupEnglish);
    }
}
