using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameHigherOrLower : MiniGameRound
{
    [SerializeField]
    private int maxRandomNumber = 9;

    [SerializeField]
    private List<MiniGameCard> cards = new List<MiniGameCard>();

    private List<int> randomNumbers = new List<int>();
    private int index = 0;
    private int value;

    public override void Start()
    {
        base.Start();

        setRandomNumbers();
        this.cards[this.index].show();
        this.index++;
    }

    private void setRandomNumbers()
    {
        for(int i = 0; i < this.cards.Count; i++)
        {
            int rand = 0;
            do
            {
                rand = Random.Range(1, (this.maxRandomNumber + 1));
            }
            while (this.randomNumbers.Contains(rand));
            this.randomNumbers.Add(rand);
            this.cards[i].setValue(rand);
        }
    }

    public void input(int value) //ON CLICK
    {
        this.cards[this.index].show();

        foreach(GameObject button in this.buttons)
        {
            button.SetActive(false);
        }

        this.value = value;
    }

    public override void checkIfWon()
    {
        if (this.value != 0)
        {
            if ((this.randomNumbers[this.index - 1] < this.randomNumbers[this.index] && this.value == 1)
             || (this.randomNumbers[this.index - 1] > this.randomNumbers[this.index] && this.value == -1)) this.setSuccess(true);
            else this.setSuccess(false);

            if (this.index < this.cards.Count) this.index++;
            else endRound();
        }
    }

}
