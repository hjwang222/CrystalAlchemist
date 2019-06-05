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
}
