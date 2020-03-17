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

    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    public void setMiniGame(List<MiniGameMatch> matches)
    {
        matches.Clear();

        foreach(MiniGameMatch match in matches)
        {
            MiniGameMatch temp = Instantiate(match);
            temp.reward.setLoot();
            this.matches.Add(temp);
        }
    }

    public void updateInternalMatches()
    {
        foreach (MiniGameMatch match in matches)
        {
            match.reward.setLoot();
        }
    }

    public List<MiniGameMatch> getMatches()
    {
        return this.matches;
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

}
