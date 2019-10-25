using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameText : MonoBehaviour
{
    [SerializeField]
    private List<Image> itemIcons = new List<Image>();

    [SerializeField]
    private SimpleSignal endGameSignal;

    private List<Item> loot = new List<Item>();

    private void OnEnable()
    {
        
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void setLoot(Item item)
    {
        this.loot.Add(item);
    }

    public void endGame()
    {
        this.endGameSignal.Raise();
    }
}
