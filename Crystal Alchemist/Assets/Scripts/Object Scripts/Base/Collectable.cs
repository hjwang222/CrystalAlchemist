using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;
using System.Collections;
using System.Collections.Generic;

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

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private bool bounceOnEnable = false;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private BounceAnimation bounceAnimation;

    private ItemStats itemStats;

    private GameObject smoke;

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

    public void SetBounce(bool value) => this.bounceOnEnable = value;

    private void OnEnable()
    {
        if (this.bounceOnEnable && this.bounceAnimation != null) this.bounceAnimation.Bounce();
    }

    private void OnDisable() => this.smoke = Instantiate(MasterManager.itemDisappearSmoke, this.transform.position, Quaternion.identity);

    private void OnDestroy()
    {
        if (this.smoke != null) Destroy(this.smoke); //ugly but working
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
                Instantiate(MasterManager.itemCollectGlitter, this.transform.position, Quaternion.identity);
            }
        }
    }

    public void SetAsTreasureItem(Transform parent)
    {
        this.bounceOnEnable = false;
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
