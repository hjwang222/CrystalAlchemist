using UnityEngine;
using TMPro;


public class MiniGameDialogbox : MonoBehaviour
{
    [SerializeField]
    private MiniGameUI miniGameUI;

    [SerializeField]
    private MiniGameSlider slider;

    [SerializeField]
    private ItemUI itemUI;

    [SerializeField]
    private MiniGamePrice priceUI;

    [SerializeField]
    private GameObject startButton;

    private bool isLoaded = false;

    private void Start()
    {
        this.slider.setDifficulty(1);
        this.isLoaded = true;
    }

    public void UpdateDialogBox(int value)
    {
        this.miniGameUI.setMatch(value);
        this.itemUI.setItem(this.miniGameUI.getMatch().loot);

        MiniGameMatch match = this.miniGameUI.getMatch();
        this.priceUI.updatePrice(match.item, match.price, this.miniGameUI.player);
        bool canStart = Utilities.Items.hasEnoughCurrency(ResourceType.item, this.miniGameUI.player, match.item, match.price);

        this.startButton.SetActive(canStart);
        this.priceUI.setColor(canStart);
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
