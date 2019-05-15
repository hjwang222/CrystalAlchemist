using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private Player player;
    public List<GameObject> boxes; 

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        int i = 0;
        while(i < boxes.Count && i < this.player.inventory.Count)
        {
            boxes[i].GetComponent<Image>().sprite = this.player.inventory[i].getSprite();
            boxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x"+this.player.inventory[i].amount + "";
            i++;
        }
    }
}
