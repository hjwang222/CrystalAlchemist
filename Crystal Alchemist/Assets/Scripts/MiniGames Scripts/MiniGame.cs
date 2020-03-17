using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MiniGame : MonoBehaviour
{
    [BoxGroup("MiniGame Related")]
    [Required]
    public MiniGameRound miniGameRound;

    [Space(10)]
    [BoxGroup("MiniGame Related")]
    public string miniGameTitle;

    [BoxGroup("MiniGame Related")]
    public string miniGameTitleEnglish;

    [Space(10)]
    [BoxGroup("MiniGame Related")]
    [TextArea]
    public string miniGameDescription;

    [BoxGroup("MiniGame Related")]
    [TextArea]
    public string miniGameDescriptionEnglish;

    private MiniGameMatches matches;

    public void setMiniGame(MiniGameMatches matches)
    {
        this.matches = Instantiate(matches);
        this.matches.Initialize();
    }

    public void updateInternalMatches()
    {
        this.matches.Initialize();
    }

    public List<MiniGameMatch> getMatches()
    {
        return this.matches.GetMatches();
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

}
