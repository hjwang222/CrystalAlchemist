using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            boxes[i] = this.player.inventory[i].gameObject;
            i++;
        }
    }
}
