using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Item")]
public class InventoryItem : ScriptableObject
{
    [Required]
    [BoxGroup("Pflichtfeld")]
    public Sprite itemSprite;

    [Required]
    [BoxGroup("Pflichtfeld")]
    public Sprite itemSpriteInventory;

    [Space(10)]
    [FoldoutGroup("Item Texts", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    [SerializeField]
    public string itemGroup;

    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public string itemGroupEnglish;

    [Space(10)]
    [Tooltip("Slot-Nummer im Inventar. Wenn -1 dann kein Platz im Inventar")]
    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public int itemSlot = -1;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int amount = 1;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int maxAmount;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [EnumToggleButtons]
    public ResourceType resourceType;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    public bool isKeyItem = false;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("isKeyItem")]
    public bool useItemGroup = false;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("isKeyItem")]
    public SimpleSignal keyItemSignal;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.skill)]
    public Ability ability;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.statuseffect)]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [FoldoutGroup("Sound", expanded: false)]
    public AudioClip collectSoundEffect;

    [FoldoutGroup("Signals", expanded: false)]
    public SimpleSignal signal;

    public void Initialize()
    {
        if (this.itemSpriteInventory == null) this.itemSpriteInventory = this.itemSprite;
    }

    public bool checkIfAlreadyThere()
    {
        if (this.isKeyItem)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null && CustomUtilities.Items.hasKeyItemAlready(this, player.inventory)) return true;
        }

        return false;
    }

    public string getItemGroup()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemGroup, this.itemGroupEnglish);
    }
}
