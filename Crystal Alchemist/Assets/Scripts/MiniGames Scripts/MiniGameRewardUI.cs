using System.Collections.Generic;
using UnityEngine;

public class MiniGameRewardUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI rewardSlot;

    [SerializeField]
    private GameObject slider;

    public void setRewardImages(List<MiniGameMatch> matches)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            rewardSlot.setItem(matches[i].reward.getLoot().item.stats);            
        }
    }

    public void setRewardSlider()
    {
        Vector2 newPosition = new Vector2(this.slider.transform.position.x, this.rewardSlot.transform.position.y);
        this.slider.transform.position = newPosition;
    }
}
