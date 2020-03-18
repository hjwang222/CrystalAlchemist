using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using UnityEditor;

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

    #region Start Funktionen

    public void SetItem(ItemStats item)
    {
        this.item = item;
    }

    private void Start()
    {
        //Check if keyItem already in Inventory
        if (this.item.alreadyThere()) Destroy(this.gameObject);
    }

    #endregion

    public void playSounds()
    {
        CustomUtilities.Audio.playSoundEffect(this.gameObject, this.item.getSoundEffect());
    }

    #region Collect Item Funktionen

    public void OnTriggerEnter2D(Collider2D character)
    {
        if (!character.isTrigger)
        {
            Player player = character.GetComponent<Player>();
            if (player != null)
            {
                player.GetComponent<PlayerUtils>().CollectItem(this.item);
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
