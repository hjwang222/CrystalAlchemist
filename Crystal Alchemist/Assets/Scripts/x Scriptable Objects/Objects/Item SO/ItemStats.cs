using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Items/Item Stats")]
public class ItemStats : ScriptableObject
{
    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    private string itemNameEnglish;

    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    private string itemName;

    [Space(10)]
    [FoldoutGroup("Item Texts", expanded: false)]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    [SerializeField]
    private string itemDescription;

    [FoldoutGroup("Item Texts", expanded: false)]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    [SerializeField]
    private string itemDescriptionEnglish;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [SerializeField]
    private int value = 1;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    public Sprite itemSprite;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private Sprite itemSpriteInventory;

    [Space(10)]
    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public ItemGroup itemGroup;

    [Space(10)]
    [Tooltip("Slot-Nummer im Inventar. Wenn -1 dann kein Platz im Inventar")]
    [FoldoutGroup("Item Texts", expanded: false)]
    [SerializeField]
    public int itemSlot = -1;

    [HideInInspector]
    public int amount = 1;

    [FoldoutGroup("Item Attributes", expanded: false)]
    public int maxAmount;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [EnumToggleButtons]
    [SerializeField]
    private ResourceType resourceType;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.item)]
    [SerializeField]
    public bool isKeyItem = false;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("isKeyItem")]
    [SerializeField]
    public SimpleSignal keyItemSignal;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.skill)]
    [SerializeField]
    private Ability ability;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [ShowIf("resourceType", ResourceType.statuseffect)]
    [SerializeField]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [FoldoutGroup("Sound", expanded: false)]
    [SerializeField]
    private AudioClip collectSoundEffect;

    [FoldoutGroup("Signals", expanded: false)]
    [SerializeField]
    public SimpleSignal signal;

    public void Initialize(int amount)
    {
        if (this.itemSpriteInventory == null) this.itemSpriteInventory = this.itemSprite;
        this.amount = amount;
    }

    public string getItemGroup()
    {
        if (this.itemGroup != null) return this.itemGroup.getItemGroup();
        else return "";
    }

    public bool alreadyThere()
    {
        if (this.isKeyItem)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null && player.GetComponent<PlayerUtils>().hasKeyItemAlready(this)) return true;
        }

        return false;
    }

    public Sprite getSprite()
    {
        if (this.itemSpriteInventory != null) return this.itemSpriteInventory;
        else return this.itemSprite;
    }

    public int getTotalAmount()
    {
        return this.value * this.amount;
    }

    public AudioClip getSoundEffect()
    {
        return this.collectSoundEffect;
    }

    public string getName()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemName, this.itemNameEnglish);        
    }

    public string getDescription()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemDescription, this.itemDescriptionEnglish);
    }

    public string getItemName(float price)
    {
        string result = "";

        switch (this.resourceType)
        {
            case ResourceType.item:
                {
                    string typ = this.getItemGroup();
                    if (price == 1 && (typ != "Schlüssel" || GlobalValues.useAlternativeLanguage)) typ = typ.Substring(0, typ.Length - 1);

                    result = typ;
                }; break;
            case ResourceType.life: result = "Leben"; break;
            case ResourceType.mana: result = "Mana"; break;
        }

        return result;
    }
}
