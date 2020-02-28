using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MiniGame : MonoBehaviour
{
    [BoxGroup("Required")]
    [Required]
    public GameObject lootParentObject;

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

    public List<MiniGameMatch> internalMatches = new List<MiniGameMatch>();


    public void setMiniGame(List<MiniGameMatch> matches)
    {
        this.matches = matches;
    }

    public void updateInternalMatches()
    {
        CustomUtilities.UnityFunctions.UpdateItemsInEditor(this.matches, this.internalMatches, this.lootParentObject);
    }

    public List<MiniGameMatch> getMatches()
    {
        return this.internalMatches;
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

}
