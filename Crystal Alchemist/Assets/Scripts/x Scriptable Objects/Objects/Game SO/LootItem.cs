using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private float amount = 1;
}
