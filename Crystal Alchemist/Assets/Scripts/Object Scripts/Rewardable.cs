using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class Rewardable : Interactable
{
    [FoldoutGroup("Loot", expanded: false)]
    [SerializeField]
    private LootTable lootTable;

    [HideInInspector]
    public ItemDrop itemDrop;

    public override void Start()
    {
        base.Start();
        setLoot();
    }

    public void setLoot()
    {
        this.itemDrop = this.lootTable.SetLoot();
    }
}
