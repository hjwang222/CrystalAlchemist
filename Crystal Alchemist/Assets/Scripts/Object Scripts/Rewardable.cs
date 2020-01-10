using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class Rewardable : Interactable
{
    [FoldoutGroup("Loot", expanded: false)]
    [Required]
    public GameObject lootParentObject;

    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Items und deren Wahrscheinlichkeit zwischen 1 und 100")]
    public List<LootTable> lootTable = new List<LootTable>();

    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Multiloot = alle Items. Ansonsten nur das seltenste Item")]
    public bool multiLoot = false;

    [HideInInspector]
    public List<Item> inventory = new List<Item>();

    public List<LootTable> lootTableInternal = new List<LootTable>();

    public override void Start()
    {
        base.Start();
        CustomUtilities.UnityFunctions.UpdateItemsInEditor(this.lootTable, this.lootTableInternal, this.lootParentObject, this.gameObject);
        CustomUtilities.Items.setItem(this.lootTableInternal, this.multiLoot, this.inventory, this.lootParentObject);
    }
}
