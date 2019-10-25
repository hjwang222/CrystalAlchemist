using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class MiniGameUI : MenuControls
{
    public MiniGameRound miniGame;
    private MiniGameRound activeRound;

    private List<MiniGameMatch> matches = new List<MiniGameMatch>();
    private MiniGameMatch match;
    private int matchIndex = 0;

    private int success = 0;
    private int winNeeded = 0;
    private int maxRounds = 0;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI timeField;
    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private Image timeImage;


    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI titleField;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    [TextArea]
    private TextMeshProUGUI descriptionField;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private GameObject mainBoard;

    private void Start()
    {
        //this.elapsed = this.maxDuration;
        //Bereit ja/nein
        //Start Animation
        //Start Game
        startRound();
    }

    public override void Update()
    {
        base.Update();
        if (this.activeRound != null)
        {
            this.timeField.text = this.activeRound.getSeconds() + "s";
            this.timeImage.fillAmount = (this.activeRound.getSeconds()*100/ this.match.maxDuration);
        }
    }

    public void setMiniGame(MiniGameRound miniGame, List<MiniGameMatch> matches, string title, string description)
    {
        this.miniGame = miniGame;
        this.matches = matches;
        this.titleField.text = title;
        this.descriptionField.text = description;
    }

    private void startRound()
    {
        if (this.activeRound != null) Destroy(this.activeRound);

        this.activeRound = Instantiate(this.miniGame, this.mainBoard.transform);
        this.match = this.matches[this.matchIndex];

        this.activeRound.setParameters(this.match.maxDuration, (this.matchIndex+1), this.match.difficulty);
    }

    private void checkIfRewarded()
    {
        if(this.success >= this.winNeeded)
        {
            //WIN! Text
            //Drop loot
            endGame();
        }
        else if((this.matchIndex+1) > this.maxRounds)
        {
            //LOSE Text
            endGame();
        }
    }

    public void endRound()
    {
        Destroy(this.activeRound.gameObject);
        this.activeRound = null;
    }

    public void setMarks(bool success) //SIGNAL
    {
        if (success)
        {
            this.success++;
            //Set Mark
            //Show Text "Success"          
        }
        else
        {
            //Set Mark
            //Show Text "Failed"
        }

        //Delay
        checkIfRewarded();
        endRound();
        //show Dialogbox
    }

    public void DialogBoxOptions(int value)
    {
        if(value == 0)
        {
            startRound();
        }
        else if (value > 0)
        {
            this.matchIndex++;
            startRound();
        }
        else
        {
            endGame();
        }
    }

    public void endGame()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        this.exitMenu();
    }

}
