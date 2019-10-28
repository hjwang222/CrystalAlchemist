using UnityEngine;
using UnityEngine.UI;

public class MiniGameDialogbox : MonoBehaviour
{
    [SerializeField]
    private MiniGameUI miniGameUI;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private ItemUI itemUI;

    private void OnEnable()
    {
        updateSlider();
    }

    public void setSlider(int matches)
    {
        this.slider.maxValue = matches;
        updateSlider();
    }

    public void startMatch()
    {
        updateSlider();
        this.miniGameUI.startMatch();
        this.hideIt();
    }

    public void endMiniGame()
    {
        this.miniGameUI.endMiniGame();
    }

    public void updateSlider()
    {        
        this.miniGameUI.setMatch((int)this.slider.value);
        this.itemUI.setItem(this.miniGameUI.getItem());
    }

    private void hideIt()
    {
        this.gameObject.SetActive(false);
    }
}
