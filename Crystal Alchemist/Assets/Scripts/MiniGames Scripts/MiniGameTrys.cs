using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTrys : MonoBehaviour
{
    [SerializeField]
    private List<MiniGameTrySlot> slots = new List<MiniGameTrySlot>();

    public int needed;
    public int max;
    public int successCounter;

    private int index;

    private void Start()
    {
        setSlots(this.index);
    }

    public void setMark(bool success)
    {
        if (success) this.successCounter++;
        this.slots[this.index].setMark(success);
        this.index++;
        setSlots(this.index);
    }

    public void setValues(int needed, int max)
    {
        this.needed = needed;
        this.max = max;
    }

    private void setSlots(int startValue)
    {
        for(int i = startValue; i < slots.Count; i++)
        {
            if ((i + 1) <= needed) slots[i].setAsNeccessary();
            if ((i + 1) <= max) slots[i].gameObject.SetActive(true);
            else slots[i].gameObject.SetActive(false);
        }
    }

}
