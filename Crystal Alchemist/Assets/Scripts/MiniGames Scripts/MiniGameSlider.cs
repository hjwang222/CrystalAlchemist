using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniGameSlider : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> stars = new List<GameObject>();

    [SerializeField]
    private MiniGameDialogbox dialogBox;


    private int value = 1;
    private int maxValue = 1;


    private void OnEnable()
    {
        this.dialogBox.resetTry();
    }

    public void setValues(int maxValue)
    {
        this.maxValue = maxValue;

        for (int i = 0; i < this.stars.Count; i++)
        {
            this.stars[i].SetActive(false);
            if (i < this.maxValue) this.stars[i].SetActive(true);
        }
    }

    public void setDifficulty(int value)
    {        
        if (this.value == value && value > 1) this.value--;
        else this.value = value;

        for (int i = 0; i < maxValue; i++)
        {
            this.stars[i].transform.GetChild(0).gameObject.SetActive(false);            
            if(i < this.value) this.stars[i].transform.GetChild(0).gameObject.SetActive(true);
        }

        this.dialogBox.UpdateDialogBox();
    }

    public int getValue()
    {
        return this.value;
    }
}
