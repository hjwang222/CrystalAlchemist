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

    [BoxGroup("Loot")]
    [Range(1, 99)]
    public int amount;

    [BoxGroup("Loot")]
    public Item alternativeLoot;

    [BoxGroup("Loot")]
    [Range(1, 99)]
    public int alternativeAmount;

    public MiniGameMatch(MiniGameMatch match)
    {
        this.maxRounds = match.maxRounds;
        this.winsNeeded = match.winsNeeded;
        this.difficulty = match.difficulty;
        this.maxDuration = match.maxDuration;
        this.price = match.price;
        this.item = match.item;
        this.amount = match.amount;
        this.alternativeAmount = match.alternativeAmount;
    }
}

public class MiniGameMachine : Interactable
{
    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGame miniGame;

    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]   
    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    public override void Start()
    {
        base.Start();
    }

    public override void doSomethingOnSubmit()
    {
        MiniGame miniGame = Instantiate(this.miniGame);
        miniGame.setMiniGame(this.matches);
    }
}
