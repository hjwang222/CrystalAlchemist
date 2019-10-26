using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameRewardUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> rewardSlots = new List<GameObject>();

    [SerializeField]
    private GameObject slider;

    public void setRewardImages(List<MiniGameMatch> matches)
    {
        for (int i = 0; i < this.rewardSlots.Count; i++)
        {
            if (matches.Count >= (i+1) && matches[i].item != null)
            {
                rewardSlots[i].GetComponent<Image>().sprite = matches[i].item.itemSprite;
            }
            else
            {
                rewardSlots[i].SetActive(false);
            }
        }
    }

    public void setRewardSlider(int value)
    {
        Vector2 newPosition = new Vector2(this.slider.transform.position.x, this.rewardSlots[value].transform.position.y);
        this.slider.transform.position = newPosition;
    }
}
