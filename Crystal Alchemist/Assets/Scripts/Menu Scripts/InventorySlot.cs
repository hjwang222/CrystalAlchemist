using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private int ID;

    private void Awake()
    {
        // this.ID = ((int.Parse(this.gameObject.transform.parent.name.Replace("Page ",""))-1)*10)+ int.Parse(this.gameObject.transform.name);
        this.ID = this.gameObject.transform.GetSiblingIndex() + 1;
    }
}
