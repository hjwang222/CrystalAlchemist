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

    [Space(10)]
    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameTitle;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameTitleEnglish;

    [Space(10)]
    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [TextArea]
    private string miniGameDescription;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [TextArea]
    private string miniGameDescriptionEnglish;

    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    private MiniGameUI activeUI;

    private void Start()
    {
        setItem();

        this.activeUI = Instantiate(this.uI, this.transform);
        this.activeUI.setMiniGame(this, this.miniGameRound, this.matches, 
                                  this.miniGameTitle, this.miniGameTitleEnglish, this.miniGameDescription, this.miniGameDescriptionEnglish);
    }

    public void setMatch(List<MiniGameMatch> matches)
    {
        foreach(MiniGameMatch match in matches)
        {
            this.matches.Add(new MiniGameMatch(match));
        }
    }

    public List<MiniGameMatch> getMatches()
    {
        return this.matches;
    }

    private void setItem()
    {
        for (int i = 0; i < this.matches.Count; i++)
        {
            //Vom Prefab lösen
            MiniGameMatch match = this.matches[i];

            Item temp = Instantiate(match.loot, this.transform);
            temp.amount = match.amount;
            temp.gameObject.SetActive(false);

            match.loot = temp;
            this.matches[i] = match;
        }
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

}
