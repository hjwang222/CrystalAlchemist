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
        startRound();
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

    private void startRound()
    {
        if (this.activeRound != null) Destroy(this.activeRound);

        this.activeRound = Instantiate(this.miniGame, this.mainBoard.transform);
        this.match = this.matches[this.matchIndex];
        this.trySlots.setValues(this.match.winsNeeded, this.match.maxRounds);

        this.activeRound.setParameters(this.match.maxDuration, (this.matchIndex+1), this.match.difficulty, this.cursor);
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
            this.trySlots.setMark(true);
            showTexts(this.successText);        
        }
        else
        {
            this.trySlots.setMark(false);
            showTexts(this.failText);
        }

        //Delay
        //checkIfRewarded();
        //endRound();
        //show Dialogbox
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

    private void checkIfRewarded()
    {
        if (this.trySlots.successCounter >= this.trySlots.needed)
        {
            //set item
            showTexts(this.winText);
            //Add loot to player
        }
        else if ((this.matchIndex + 1) > this.trySlots.max)
        {
            showTexts(this.loseText);
        }
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
