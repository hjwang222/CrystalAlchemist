using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

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
    [ShowIf("resourceType", ResourceType.skill)]
    [SerializeField]
    private Ability ability;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", ResourceType.statuseffect)]
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

    [BoxGroup("Signals")]
    [SerializeField]
    public SimpleSignal signal;



    public ItemStats getInventoryItem(int ID, bool keyItem)
    {
        if (this.itemGroup != null
            && this.itemGroup.inventoryItem
            && this.itemGroup.isKeyItem == keyItem
            && this.itemGroup.itemSlot == ID) return this;

        return null;
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

    public void raiseKeyItemSignal()
    {
        if (this.itemGroup != null) this.itemGroup.raiseKeySignal();
    }

    public bool alreadyThere()
    {
        if (isKeyItem())
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null && player.GetComponent<PlayerUtils>().hasKeyItemAlready(this)) return true;
        }

        return false;
    }

    public Sprite getSprite()
    {
        return this.itemGroup.getSprite();
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
