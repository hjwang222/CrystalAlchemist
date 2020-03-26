using System.Collections;
using System.Collections.Generic;
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

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private TextMeshPro priceText;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private Color fontColor;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private Color outlineColor;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private float outlineWidth = 0.25f;
    #endregion


    #region Start Funktionen

    public override void Start()
    {
        base.Start();
        this.setLoot();
        FormatUtil.set3DText(this.priceText, this.costs.amount + "", true, this.fontColor, this.outlineColor, this.outlineWidth);

        if (this.itemDrop == null && this.treasureType == TreasureType.normal) changeTreasureState(true);
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
        if (this.currentState == objectState.showItem
            && ((this.player != null && this.player.currentState == CharacterState.interact)
              || this.player == null))
        {
            closeChest(); //close Chest
        }
    }

    public override void doSomethingOnSubmit()
    {
        if (this.currentState == objectState.normal)
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
        changeTreasureState(true);
        AudioUtil.playSoundEffect(this.gameObject, this.soundEffect);

        if (this.soundEffect != null && this.itemDrop != null)
        {
            //Spiele Soundeffekte ab            
            AudioUtil.playSoundEffect(this.gameObject, this.soundEffectTreasure, GlobalValues.backgroundMusicVolume);

            //Zeige Item
            this.showTreasureItem();

            this.itemDrop.stats.CollectIt(this.player);
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.success, this.itemDrop.stats);
        }
        else
        {
            //Kein Item drin
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.empty);
        }

        StartCoroutine(delayCo());
    }

    private void closeChest()
    {
        //Close when opened

        Destroy(this.itemDrop);

        if (this.treasureType == TreasureType.lootbox)
        {
            changeTreasureState(false);
            this.setLoot();
        }

        //Verstecke gezeigtes Item wieder
        for (int i = 0; i < this.showItem.transform.childCount; i++)
        {
            Destroy(this.showItem.transform.GetChild(i).gameObject);
        }
        this.showItem.SetActive(false);

    }

    private void changeTreasureState(bool openChest)
    {
        if (openChest)
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", true);
            this.currentState = objectState.opened;
            this.context.SetActive(false);
        }
        else
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
            this.currentState = objectState.normal;
            this.context.SetActive(true);
        }
    }


    public void showTreasureItem()
    {
        //Item instanziieren und der Liste zurück geben und das Item anzeigen            
        this.showItem.SetActive(true);

        Collectable collectable = this.itemDrop.Instantiate(this.showItem.transform.position);
        collectable.SetAsTreasureItem(this.showItem.transform);
    }

    private IEnumerator delayCo()
    {
        //BUG: Item verschwindet zu schnell, daher ein Delay setzen! Siehe DoOnUpdate -> Close Chest
        yield return new WaitForSeconds(1f);
        this.currentState = objectState.showItem;
    }

    #endregion
}
