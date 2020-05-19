using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MiniGame : MonoBehaviour
{
    [BoxGroup("MiniGame Related")]
    [Required]
    public MiniGameRound miniGameRound;

    private MiniGameMatches matches;

    public string GetName()
    {
        return FormatUtil.GetLocalisedText(this.gameObject.name+"_Name", LocalisationFileType.minigames);
    }

    public string GetDescription()
    {
        return FormatUtil.GetLocalisedText(this.gameObject.name + "_Description", LocalisationFileType.minigames);
    }

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
