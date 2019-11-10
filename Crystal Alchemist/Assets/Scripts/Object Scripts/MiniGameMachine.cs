using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

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

    public override void doSomethingOnSubmit()
    {
        MiniGame temp = Instantiate(this.miniGame);
        temp.setMatch(this.matches);
    }
}
