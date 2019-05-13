using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryCurrency : MonoBehaviour
{
    [Header("Text-Attribute")]
    public TextMeshProUGUI TMPObject;
    public Color fontColor;
    public Color outlineColor;
    public float outlineWidth = 0.25f;
    public bool bold = false;
    public ResourceType resourceType;
    public ItemGroup ItemGroup;

    private Player player;

    /*
     * 1. Inventory UI Update Signal 
     * 2. Inventory close/open Button
     * 3. Must Have (Skills, Currencys, etc)
     * 4. String Format und Background und Animationen
     * 5. Stats?
     */

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();

        LoadInventory();
    }

    private void OnEnable()
    {
        //loadInventory();
    }

    public void LoadInventory()
    {
        if (this.player != null)
        {
            string text = this.player.getResource(this.resourceType, null) + "";
            Utilities.set3DText(this.TMPObject, text, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }
}
