using UnityEngine;
using System.Collections.Generic;
using TMPro;

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
    private GameObject startButton;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private bool isLoaded = false;

    private void Start()
    {
        this.slider.setDifficulty(1);
        this.isLoaded = true;
    }

    public void UpdateDialogBox(int value)
    {
        this.miniGameUI.setMatch(value);

        foreach (ItemUI itemUI in this.itemUIs)
        {
            itemUI.setItem(this.miniGameUI.getMatch().loot);
        }

        MiniGameMatch match = this.miniGameUI.getMatch();
        this.priceUI.updatePrice(match.item, match.price, this.miniGameUI.player);
        bool canStart = CustomUtilities.Items.hasEnoughCurrency(ResourceType.item, this.miniGameUI.player, match.item, match.price);

        this.startButton.SetActive(canStart);
        this.priceUI.setColor(canStart);

        string text = this.miniGameUI.miniGameRound.getDifficulty(this.miniGameUI.mainDescription, match.difficulty);
        this.descriptionText.text = text;
    }    

    private void OnEnable()
    {
        if(this.isLoaded) UpdateDialogBox(this.slider.getValue());
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
