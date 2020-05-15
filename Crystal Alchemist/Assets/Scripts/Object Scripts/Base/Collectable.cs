using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using UnityEditor;
using AssetIcons;

public class Collectable : MonoBehaviour
{
    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private SpriteRenderer shadowRenderer;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private ItemDrop itemDrop;

    private ItemStats itemStats;

    [AssetIcon]
    private Sprite GetSprite()
    {
        return this.GetComponent<SpriteRenderer>().sprite;
    }

    #region Start Funktionen

    public void SetItem(ItemDrop drop)
    {
        this.itemDrop = drop;
        setItemStats();
    }

    private void setItemStats()
    {
        ItemStats temp = Instantiate(this.itemDrop.stats);
        temp.name = this.itemDrop.name;
        this.itemStats = temp;
    }

    private void Start()
    {
        //Check if keyItem already in Inventory
        setItemStats();
        if (this.itemStats.alreadyThere()) DestroyIt();
    }

    #endregion

    public void playSounds()
    {
        AudioUtil.playSoundEffect(this.gameObject, this.itemStats.getSoundEffect());
    }

    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (!character.isTrigger)
        {
            Player player = character.GetComponent<Player>();
            if (player != null)
            {
                GameEvents.current.DoCollect(this.itemStats);
                playSounds();
                DestroyIt();
            }
        }
    }

    public void SetAsTreasureItem(Transform parent)
    {
        this.transform.parent = parent;
        this.transform.position = parent.position;
        if (this.shadowRenderer != null) this.shadowRenderer.enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
