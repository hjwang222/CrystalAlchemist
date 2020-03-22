using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Items/Inventory Info")]
public class InventoryInfo : ScriptableObject
{
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

    public void raiseKeySignal()
    {
        if (this.keyItemSignal != null) this.keyItemSignal.Raise();
    }

    public void raiseCollectSignal()
    {
        if (this.collectSignal != null) this.collectSignal.Raise();
    }

    public Sprite getSprite()
    {
        return this.inventorySprite;
    }

    public string getDescription()
    {
        return FormatUtil.getLanguageDialogText(this.description, this.descriptionEnglish);
    }
}
