using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public enum TreasureType
{
    normal,
    lootbox
}

public class Treasure : Rewardable
{
    #region Attribute   

    [BoxGroup("Treasure Options")]
    [Required]
    [SerializeField]
    private Animator anim;

    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private GameObject showItem;

    [BoxGroup("Treasure Options")]
    [SerializeField]
    private AudioClip soundEffectTreasure;

    [BoxGroup("Treasure Options")]
    [SerializeField]
    private TreasureType treasureType = TreasureType.normal;

    [BoxGroup("Loot")]
    [SerializeField]
    private bool useLootTable = false;

    [BoxGroup("Loot")]
    [HideIf("useLootTable")]
    [SerializeField]
    [HideLabel]
    private Reward reward;

    [BoxGroup("Loot")]
    [ShowIf("useLootTable")]
    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    [BoxGroup("Mandatory")]
    [Required]
    private ShopPrice shopPrice;

    #endregion

    private bool treasureEnabled = true;

    #region Start Funktionen

    public override void Start()
    {
        base.Start();
        this.setLoot();
        this.shopPrice.Initialize(this.costs);

        if (this.itemDrop == null 
        && this.treasureType == TreasureType.normal) ChangeTreasureState(true);

        AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
        analyse.SetTarget(this.gameObject);       
    }

    private void setLoot()
    {
        if (this.useLootTable) this.itemDrop = this.lootTable.GetItemDrop();
        else this.itemDrop = this.reward.GetItemDrop();
    }

    #endregion


    #region Update Funktion

    public override void doOnUpdate()
    {
        if (!this.treasureEnabled
            && ((this.player != null && this.player.values.currentState == CharacterState.interact)
              || this.player == null))
        {
            closeChest(); //close Chest
        }
    }

    public override void doSomethingOnSubmit()
    {
        if (this.treasureEnabled)
        {
            if (this.player.canUseIt(this.costs)) openChest(); //open Chest
            else this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.failed);
        }
    }

    #endregion


    #region Treasure Chest Funktionen (open, show Item)

    private void openChest()
    {
        this.player.reduceResource(this.costs);
        ChangeTreasureState(true);

        if (this.soundEffect != null && this.itemDrop != null)
        {
            //Zeige Item
            this.showTreasureItem();
   
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.success, this.itemDrop.stats);
        }
        else
        {
            //Kein Item drin
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.empty);
        }
    }

    private void closeChest()
    {
        //Close when opened
        Destroy(this.itemDrop);        

        if (this.treasureType == TreasureType.lootbox)
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
            this.setLoot();
        }

        //Verstecke gezeigtes Item wieder
        for (int i = 0; i < this.showItem.transform.childCount; i++)
        {
            Destroy(this.showItem.transform.GetChild(i).gameObject);
        }

        this.showItem.SetActive(false);
    }

    private void ChangeTreasureState(bool openChest)
    {
        AudioUtil.playSoundEffect(this.gameObject, this.soundEffect);
        AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", openChest);       
    }

    public void SetEnabled(bool enable)
    {
        //Animator Events
        this.treasureEnabled = enable;
        this.context.gameObject.SetActive(enable);
    }

    public void showTreasureItem()
    {
        AudioUtil.playSoundEffect(this.gameObject, this.soundEffectTreasure, MasterManager.settings.backgroundMusicVolume);

        //Item instanziieren und der Liste zurück geben und das Item anzeigen            
        this.showItem.SetActive(true);

        Collectable collectable = this.itemDrop.Instantiate(this.showItem.transform.position);
        collectable.SetAsTreasureItem(this.showItem.transform);

        this.itemDrop.stats.CollectIt(this.player);
    }

    #endregion
}
