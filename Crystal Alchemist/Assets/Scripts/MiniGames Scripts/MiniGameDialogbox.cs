using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MiniGameDialogbox : MonoBehaviour
{
    [SerializeField]
    private MiniGameUI miniGameUI;

    [SerializeField]
    private MiniGameSlider slider;

    [SerializeField]
    private List<ItemUI> itemUIs = new List<ItemUI>();

    [SerializeField]
    private MiniGamePrice priceUI;

    [SerializeField]
    private Selectable startButton;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private bool isLoaded = false;

    private void Start()
    {
        this.slider.setDifficulty(1);
        this.isLoaded = true;
    }

    public void UpdateDialogBox()
    {        
        this.miniGameUI.setMatch(this.slider.getValue());

        foreach (ItemUI itemUI in this.itemUIs)
        {
            itemUI.setItem(this.miniGameUI.getMatch().loot);
        }

        MiniGameMatch match = this.miniGameUI.getMatch();
        bool enableStart = this.priceUI.updatePrice(match.currencyType, match.item, match.price, this.miniGameUI.player);
        this.startButton.interactable = enableStart;

        string text = this.miniGameUI.miniGameRound.getDifficulty(this.miniGameUI.mainDescription, match.difficulty);
        this.descriptionText.text = text;
    }    

    private void OnEnable()
    {
        if(this.isLoaded) UpdateDialogBox();
    }

    public void resetTry()
    {
        this.miniGameUI.resetTrys();
    }

    public void setValues(int matches)
    {
        this.slider.setValues(matches);
    }

    public void startMatch()
    {
        this.miniGameUI.startMatch();
        this.hideIt();
    }

    public void endMiniGame()
    {
        this.miniGameUI.endMiniGame();
    }

    private void hideIt()
    {
        this.gameObject.SetActive(false);
    }
}
