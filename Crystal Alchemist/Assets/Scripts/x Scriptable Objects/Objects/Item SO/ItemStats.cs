using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using AssetIcons;



[CreateAssetMenu(menuName = "Game/Items/Item Stats")]
public class ItemStats : ScriptableObject
{
    [BoxGroup("Attributes")]
    [SerializeField]
    private int value = 1;

    [BoxGroup("Attributes")]
    public CostType resourceType;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", CostType.none)]
    [SerializeField]
    private Ability ability;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", CostType.none)]
    [SerializeField]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [BoxGroup("Inventory")]
    [SerializeField]
    [HideIf("inventoryInfo")]
    [HideIf("resourceType", CostType.life)]
    [HideIf("resourceType", CostType.mana)]
    [HideIf("resourceType", CostType.none)]
    public ItemGroup itemGroup;

    [BoxGroup("Inventory")]
    [SerializeField]
    [Required]
    public ItemInfo info;

    [BoxGroup("Inventory")]
    [SerializeField]
    [Required]
    [HideIf("itemGroup")]
    [HideIf("resourceType", CostType.life)]
    [HideIf("resourceType", CostType.mana)]
    [HideIf("resourceType", CostType.none)]
    public ItemSlotInfo inventoryInfo;

    [HideInInspector]
    public int amount = 1;

    [BoxGroup("Signals")]
    [SerializeField]
    private AudioClip collectSoundEffect;
    



    public ItemInfo getInfo()
    {
        //if (this.itemGroup != null) return this.itemGroup.info;
        return this.info;
    }

    public bool isID(int ID)
    {
        if (this.itemGroup != null) return this.itemGroup.isID(ID);
        else if (this.inventoryInfo != null) return this.inventoryInfo.isID(ID);
        return false;
    }

    

    public void Initialize(int amount)
    {
        this.amount = amount;
    }

    public string getItemGroup()
    {
        if (this.itemGroup != null) return this.itemGroup.getName();
        else return "";
    }

    public bool isKeyItem()
    {
        if (this.itemGroup != null) return this.itemGroup.isKeyItem();
        else if (this.inventoryInfo != null) return this.inventoryInfo.isKeyItem();
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

    [AssetIcon]
    public Sprite getSprite()
    {
        if(this.info != null) return this.info.getSprite();
        return null;
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
        return this.info.getName();
    }

    public string getDescription()
    {
        return this.info.getDescription();
    }    
}
