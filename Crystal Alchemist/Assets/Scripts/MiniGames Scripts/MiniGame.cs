using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct MiniGameMatch
{
    [Range(1, 6)]
    public int maxRounds;

    [Range(1, 6)]
    public int winsNeeded;

    [Range(1, 5)]
    public int difficulty;

    [Range(1, 120)]
    public float maxDuration;

    public Item item;

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




    private void Awake()
    {
        this.activeUI = Instantiate(this.uI, this.transform);
        this.activeUI.setMiniGame(this.miniGameRound, this.matches, this.miniGameTitle, this.miniGameDescription);
    }

    private void Start()
    {
        
    }
}
