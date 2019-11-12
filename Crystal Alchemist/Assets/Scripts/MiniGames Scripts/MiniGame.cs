using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
    private string miniGameTitle;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameTitleEnglish;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameDescription;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameDescriptionEnglish;

    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    private MiniGameUI activeUI;

    private void Start()
    {
        this.activeUI = Instantiate(this.uI, this.transform);
        this.activeUI.setMiniGame(this, this.miniGameRound, this.matches, this.miniGameTitle, this.miniGameDescription);
    }

    public void setMatch(List<MiniGameMatch> matches)
    {
        this.matches = matches;
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

}
