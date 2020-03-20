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
    private ItemStats item;

    [AssetIcon]
    private Sprite GetSprite()
    {
        return this.GetComponent<SpriteRenderer>().sprite;
    }

    #region Start Funktionen

    public void SetItem(ItemStats item)
    {
        this.item = item;
    }

    private void Start()
    {
        //Check if keyItem already in Inventory
        if (this.item.alreadyThere()) DestroyIt();
    }

    #endregion

    public void playSounds()
    {
        AudioUtil.playSoundEffect(this.gameObject, this.item.getSoundEffect());
    }

    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (!character.isTrigger)
        {
            Player player = character.GetComponent<Player>();
            if (player != null)
            {
                this.item.CollectIt(player);
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
