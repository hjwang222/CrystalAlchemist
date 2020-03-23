using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using AssetIcons;

[HideMonoScript]
[CreateAssetMenu(menuName = "Game/Items/Item Stats")]
public class ItemStats : ScriptableObject
{
    [BoxGroup("Item Texts")]
    [SerializeField]
    private string itemName;

    [BoxGroup("Item Texts")]
    [SerializeField]
    private string itemNameEnglish;

    [Space(10)]
    [BoxGroup("Item Texts")]
    [TextArea]
    [SerializeField]
    private string itemDescription;

    [BoxGroup("Item Texts")]
    [TextArea]
    [SerializeField]
    private string itemDescriptionEnglish;

    [BoxGroup("Attributes")]
    [SerializeField]
    private int value = 1;

    [BoxGroup("Attributes")]
    [EnumToggleButtons]
    [SerializeField]
    private ResourceType resourceType;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", ResourceType.none)]
    [SerializeField]
    private Ability ability;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", ResourceType.none)]
    [SerializeField]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [BoxGroup("Inventory")]
    [SerializeField]
    [ShowIf("resourceType", ResourceType.item)]
    public ItemGroup itemGroup;           

    [HideInInspector]
    public int amount = 1;

    [BoxGroup("Signals")]
    [SerializeField]
    private AudioClip collectSoundEffect;
    

    [BoxGroup("Unity Icon")]
    [InfoBox("To show Icon in Unity Inspector. Not neccessary", InfoMessageType = InfoMessageType.Info)]
    [AssetIcon]
    [SerializeField]
    private Sprite icon;


    public bool isID(int ID)
    {
        if (this.itemGroup != null && this.itemGroup.itemSlot == ID) return true;
        return false;
    }

    public void CollectIt(Player player)
    {
        //Collectable, Load, MiniGame, Shop und Treasure

        if (this.resourceType == ResourceType.life || this.resourceType == ResourceType.mana) player.updateResource(this.resourceType, this.amount, true);
        else if (this.resourceType == ResourceType.item) player.GetComponent<PlayerItems>().CollectInventoryItem(this);
        else if (this.resourceType == ResourceType.none)
        {
            //if(this.ability != null)
            foreach (StatusEffect effect in this.statusEffects)
            {
                StatusEffectUtil.AddStatusEffect(effect, player);
            }
        }
    }

    public void Initialize(int amount)
    {
        this.amount = amount;
    }

    public string getItemGroup()
    {
        if (this.itemGroup != null) return this.itemGroup.getItemGroup();
        else return "";
    }

    public bool isKeyItem()
    {
        if (this.itemGroup != null && this.itemGroup.isKeyItem) return true;
        return false;
    }

    public int getMaxAmount()
    {
        if (this.itemGroup != null) return this.itemGroup.maxAmount;
        return 0;
    }

    public bool alreadyThere()
    {
        if (isKeyItem())
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null && player.GetComponent<PlayerItems>().hasKeyItemAlready(this)) return true;
        }

        return false;
    }

    public Sprite getSprite()
    {
        return this.icon;
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
        return FormatUtil.getLanguageDialogText(this.itemName, this.itemNameEnglish);        
    }

    public string getDescription()
    {
        return FormatUtil.getLanguageDialogText(this.itemDescription, this.itemDescriptionEnglish);
    }

    
}
