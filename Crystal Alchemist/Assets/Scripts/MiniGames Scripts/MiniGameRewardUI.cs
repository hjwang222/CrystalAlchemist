using System.Collections.Generic;
using UnityEngine;

public class MiniGameRewardUI : MonoBehaviour
{
    [SerializeField]
    private List<ItemUI> rewardSlots = new List<ItemUI>();

    [SerializeField]
    private GameObject slider;

    public void setRewardImages(List<MiniGameMatch> matches)
    {
        for (int i = 0; i < this.rewardSlots.Count; i++)
        {
            if (matches.Count >= (i+1) && matches[i].reward != null)
            {
                rewardSlots[i].setItem(matches[i].reward.getLoot().item.stats);
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void setRewardSlider(int value)
    {
        Vector2 newPosition = new Vector2(this.slider.transform.position.x, this.rewardSlots[value].transform.position.y);
        this.slider.transform.position = newPosition;
    }
}
