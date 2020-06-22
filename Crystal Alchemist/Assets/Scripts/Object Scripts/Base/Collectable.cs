using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class Collectable : MonoBehaviour
{
    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private SpriteRenderer shadowRenderer;

    [Required]
    [BoxGroup("Pflichtfeld")]
    [SerializeField]
    private SpriteRenderer itemSprite;

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

    //[Required]
    //[BoxGroup("Pflichtfeld")]
    //[SerializeField]
    //private bool useUniqueName = false;

    private ItemStats itemStats;
    private bool hasDuration = false;
    private float elapsed;
    private bool showSmoke = true;

    private float blinkDelay;
    private float blinkElapsed;
    private float blinkSpeed = 0.1f;

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
        setItemStats();

        string itemName = this.itemDrop.name;
        //if(this.useUniqueName) itemName = this.gameObject.name;

        if (this.itemStats.IsKeyItem() && GameEvents.current.HasKeyItem(itemName))
        {
            this.showSmoke = false;
            DestroyIt();
        }
    }

    private void Update()
    {
        if (!this.hasDuration) return;

        if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
        else DestroyIt();

        BlinkAnimation();
    }

    private void BlinkAnimation()
    {
        if (this.elapsed < 3) this.blinkDelay = 0.25f;
        else if (this.elapsed < 6) this.blinkDelay = 0.5f;
        else if (this.elapsed <= 10) this.blinkDelay = 1f;

        if (this.blinkDelay > 0 && this.itemSprite != null)
        {
            if (this.blinkElapsed > 0) this.blinkElapsed -= Time.deltaTime;
            else
            {
                this.blinkElapsed = this.blinkDelay;
                StartCoroutine(BlinkCo(blinkSpeed));
            }
        }
    }

    private IEnumerator BlinkCo(float delay)
    {
        this.itemSprite.DOColor(new Color(0,0,0,0), delay);
        yield return new WaitForSeconds(delay);
        this.itemSprite.DOColor(Color.white, delay);
    }

    public void SetBounce(bool value) => this.bounceOnEnable = value;

    private void OnEnable()
    {
        if (this.bounceOnEnable && this.bounceAnimation != null) this.bounceAnimation.Bounce();
    }

    [Button]
    public void SetSelfDestruction()
    {
        if (this.itemStats.IsKeyItem()) return;
        this.hasDuration = true;
        this.elapsed = 60;
    }

    private void OnDisable()
    {
        if (this.showSmoke) AnimatorUtil.ShowSmoke(this.transform);
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
                this.showSmoke = false;
                GameEvents.current.DoCollect(this.itemStats);
                playSounds();
                DestroyIt();
                Instantiate(MasterManager.itemCollectGlitter, this.transform.position, Quaternion.identity);
            }
        }
    }

    public void SetAsTreasureItem(Transform parent)
    {
        this.showSmoke = false;
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
