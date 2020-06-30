using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollBarExtension : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private Scrollbar scrollBar;

    [SerializeField]
    private CustomCursor cursor;    

    public void OnSelect(BaseEventData eventData)
    {
        int value = Convert.ToInt32(this.gameObject.name.Split(':')[0].Replace("Item ", ""));
        SetScrollBar(value);
        cursor.SetTransform((RectTransform)this.transform);
        cursor.UpdatePosition();
    }

    private int GetCount()
    {
        return this.transform.parent.childCount - 1;
    }

    public void SetScrollBar(int item)
    {
        if (this.scrollBar == null) return;
        float index = (float)item;

        float value = 0f;
        if (index <= 1) value = 1f;
        else if (index <= GetCount() - 2)
        {
            float inverse = ((float)GetCount() - index) - 2f;
            value = inverse * 0.15f;
        }

        this.scrollBar.value = value;
    }

    private void OnDestroy()
    {
        cursor.SetPositionToSelectable(false);
    }
}
