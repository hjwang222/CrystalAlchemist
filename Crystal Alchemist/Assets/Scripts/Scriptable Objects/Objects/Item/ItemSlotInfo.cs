using UnityEngine;
using Sirenix.OdinInspector;

public enum ItemType
{
    inventory,
    important
}

[CreateAssetMenu(menuName = "Game/Items/Inventory Info")]
public class ItemSlotInfo : ScriptableObject
{
    [BoxGroup("Inventory")]
    [SerializeField]
    private ItemType itemType = ItemType.inventory;

    [BoxGroup("Inventory")]
    [SerializeField]
    [MinValue(-1)]
    private int itemSlot = -1;

    [BoxGroup("Inventory")]
    [ShowIf("itemType", ItemType.important)]
    [SerializeField]
    private SimpleSignal keyItemSignal;


    public bool isKeyItem()
    {
        return (this.itemType == ItemType.important);
    }

    public bool isID(int ID)
    {
        if (this.itemSlot == ID) return true;
        else return false;
    }

    public void raiseKeySignal()
    {
        if (this.keyItemSignal != null) this.keyItemSignal.Raise();
    }
}
