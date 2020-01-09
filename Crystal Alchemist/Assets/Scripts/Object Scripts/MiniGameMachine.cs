using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

[System.Serializable]
public class MiniGameMatch
{
    [Title("$difficulty", "", bold: true)]
    [BoxGroup("$difficulty", ShowLabel = false)]
    [Range(1, 6)]
    public int maxRounds;

    [BoxGroup("$difficulty")]
    [Range(1, 6)]
    public int winsNeeded;

    [BoxGroup("$difficulty")]
    [Range(1, 5)]
    public int difficulty;

    [BoxGroup("$difficulty")]
    [Range(1, 120)]
    public float maxDuration;

    [BoxGroup("Price")]
    public Item item;

    [BoxGroup("Price")]
    [Range(1, 99)]
    public int price;

    [BoxGroup("Loot")]
    public Item loot;
}

public class MiniGameMachine : Interactable
{
    [BoxGroup("Required")]
    [SerializeField]
    [Required]
    private GameObject lootParentObject;

    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGame miniGame;

    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]   
    private List<MiniGameMatch> matches = new List<MiniGameMatch>();


#if UNITY_EDITOR
    [Button]
    [BoxGroup("Mandatory")]
    private void UpdateItems()
    {
        CustomUtilities.UnityFunctions.UpdateItemsInEditor(this.matches, this.lootParentObject, this.gameObject);
    }
#endif


    public override void doSomethingOnSubmit()
    {
        MiniGame miniGame = Instantiate(this.miniGame);
        miniGame.setMiniGame(this.matches);
    }
}
