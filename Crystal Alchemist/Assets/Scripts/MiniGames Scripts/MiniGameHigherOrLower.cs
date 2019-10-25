using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameHigherOrLower : MiniGameRound
{
    [SerializeField]
    private int maxRandomNumber = 9;

    [SerializeField]
    private List<TextMeshProUGUI> fields = new List<TextMeshProUGUI>();

    private List<int> randomNumbers = new List<int>();
    private int index = 0;

    public override void Start()
    {
        base.Start();

        setRandomNumbers();
        this.fields[this.index].text = this.randomNumbers[this.index] + "";
        this.index++;
    }

    private void setRandomNumbers()
    {
        for(int i = 0; i < this.fields.Count; i++)
        {
            int rand = 0;
            do
            {
                rand = Random.Range(1, (this.maxRandomNumber + 1));
            }
            while (this.randomNumbers.Contains(rand));
            this.randomNumbers.Add(rand);
        }
    }

    public void check(bool higher) //ON CLICK
    {
        //Show Animation
        //Delay
        //Check
        this.fields[this.index].text = this.randomNumbers[this.index] + "";

        if ((this.randomNumbers[this.index - 1] < this.randomNumbers[this.index] && higher) 
         || (this.randomNumbers[this.index - 1] > this.randomNumbers[this.index] && !higher)) this.setSuccess(true);
        else this.setSuccess(false);

        if (this.index < this.fields.Count) this.index++;
        else endRound();
    }

}
