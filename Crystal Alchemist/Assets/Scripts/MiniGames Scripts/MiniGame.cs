using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct MiniGameMatch
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

public class MiniGame : MonoBehaviour
{
    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [Required]
    private MiniGameUI uI;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [Required]
    private MiniGameRound miniGameRound;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameTitle;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameDescription;

    private MiniGameUI activeUI;

    private void Start()
    {
        this.activeUI = Instantiate(this.uI, this.transform);
        this.activeUI.setMiniGame(this, this.miniGameRound, this.matches, this.miniGameTitle, this.miniGameDescription);
    }



}
