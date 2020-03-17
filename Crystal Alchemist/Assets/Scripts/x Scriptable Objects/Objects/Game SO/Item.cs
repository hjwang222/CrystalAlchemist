using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Item : ScriptableObject
{
    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private InventoryItem inventoryItem;

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
    public string itemDescription;

    [FoldoutGroup("Item Texts", expanded: false)]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string itemDescriptionEnglish;

    [FoldoutGroup("Item Attributes", expanded: false)]
    [SerializeField]
    private int value = 1;

    public bool alreadyThere()
    {
        return this.inventoryItem.checkIfAlreadyThere();
    }

    public int getTotalAmount()
    {
        return this.value * this.inventoryItem.amount;
    }

    public AudioClip getSoundEffect()
    {
        return this.inventoryItem.collectSoundEffect;
    }

    public InventoryItem GetInventoryItem()
    {
        return this.inventoryItem;
    }

    public string getName()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemName, this.itemNameEnglish);        
    }

    public string getDescription()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemDescription, this.itemDescriptionEnglish);
    }

    public string getItemGroup()
    {
        return this.inventoryItem.getItemGroup();
    }
}
