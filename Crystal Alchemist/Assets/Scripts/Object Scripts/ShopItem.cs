using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : Interactable
{
    public Item item;
    public int price;
    public TextMeshProUGUI priceText;
    public SpriteRenderer childSprite;
    
    private void Start()
    {
        init();

        this.priceText.text = price + "";
        this.childSprite.sprite = this.item.getSprite();
    }

    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("Submit"))
        {
            Player player = this.character.GetComponent<Player>();

            if (player != null)
            {
                if (player.crystals >= price)
                {                   
                    //TODO: Zu viele Käufe pro Klicks
                    //TODO: Lootbox
                    player.showDialogBox("Du hast 1 "+this.item.itemName+" für "+this.price+" Kristalle gekauft!");
                    player.crystals -= price;
                    item.GetComponent<Item>().collect(player, false);
                }
                else
                {
                    player.showDialogBox("No money, no funny");
                }
            }
        }
    }
}
