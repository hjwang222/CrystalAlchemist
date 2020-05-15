using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownExtension : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private TMP_Dropdown dropDown;

    [SerializeField]
    private Scrollbar scrollBar;

    public void OnSelect(BaseEventData eventData)
    {
        int value = Convert.ToInt32(this.gameObject.name.Split(':')[0].Replace("Item ", ""));
        SetScrollBar(value);
    }

    public void SetScrollBar(int item)
    {
        float index = (float)item;

        float value = 0f;
        if (index <= 1) value = 1f;
        else if (index <= this.dropDown.options.Count - 2)
        {
            float inverse = ((float)this.dropDown.options.Count - index) - 2f;
            value = inverse * 0.15f;
        }

        this.scrollBar.value = value;
    }
}
