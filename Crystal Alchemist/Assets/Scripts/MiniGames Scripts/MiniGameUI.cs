using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class MiniGameUI : MenuControls
{
    public MiniGameRound miniGame;

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
    private TextMeshProUGUI descriptionField;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private GameObject mainBoard;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private GameObject textGameObject;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private MiniGameTrys trySlots;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private MiniGameRewardUI rewardUI;


    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText successText;

    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText failText;

    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText winText;

    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText loseText;

    private MiniGameRound activeRound;
    private List<MiniGameMatch> matches = new List<MiniGameMatch>();
    private MiniGameMatch match;
    private int matchIndex = 0;

    private void Start()
    {
        //this.elapsed = this.maxDuration;
        //Bereit ja/nein
        //Start Animation
        //Start Game
        this.rewardUI.setRewardImages(this.matches);
        startMatch(false);
    }

    public override void Update()
    {
        base.Update();
        if (this.activeRound != null)
        {
            this.timeField.text = this.activeRound.getSeconds() + "s";
            this.timeImage.fillAmount = (float)((float)this.activeRound.getSeconds()/ this.match.maxDuration);
        }
    }

    public void setMiniGame(MiniGameRound miniGame, List<MiniGameMatch> matches, string title, string description)
    {
        this.miniGame = miniGame;
        this.matches = matches;
        this.titleField.text = title;
        this.descriptionField.text = description;
    }

    public void startRound()
    {
        MiniGameState state = this.trySlots.canStartNewRound();

        if (state == MiniGameState.play)
        {
            if (this.activeRound != null) Destroy(this.activeRound);

            this.activeRound = Instantiate(this.miniGame, this.mainBoard.transform);
            this.activeRound.setParameters(this.match.maxDuration, (this.matchIndex + 1), this.match.difficulty, this.cursor, this);
        }
        else 
        {
            if (state == MiniGameState.win)
            {
                showTexts(this.winText);
                //Show Dialog if start new match or new round or get loot (quit)
            }
            else if (state == MiniGameState.lose)
            {
                showTexts(this.loseText); //no reward
                //Show Dialog if start again or quit
            }
        }
    }

    public void startMatch(bool increaseDifficulty)
    {
        this.match = this.matches[this.matchIndex];
        this.trySlots.reset();
        this.trySlots.setValues(this.match.winsNeeded, this.match.maxRounds);
        startRound();

        if (increaseDifficulty) this.matchIndex++;
        this.rewardUI.setRewardSlider(this.matchIndex);
    }

    public void setMarkAndEndRound(bool success) //SIGNAL
    {
        if (success)
        {
            this.trySlots.updateSlots(true);
            if(this.trySlots.canStartNewRound() == MiniGameState.play) showTexts(this.successText);        
        }
        else
        {
            this.trySlots.updateSlots(false);
            if (this.trySlots.canStartNewRound() == MiniGameState.play) showTexts(this.failText);
        }

        startRound();
    }

    private void showTexts(MiniGameText textObject)
    {
        showTexts(textObject, null);
    }

    private void showTexts(MiniGameText textObject, Item item)
    {
        if (textObject != null)
        {
            textObject.gameObject.SetActive(true);
            if (item != null) textObject.setLoot(item); 
        }
    }

    public void DialogBoxOptions(int value)
    {
        if(value == 0)
        {
            startMatch(false);
        }
        else if (value > 0)
        {
            startMatch(true);
        }
        else
        {
            endGame();
            //get loot
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
