using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameHigherOrLower : MiniGameRound
{
    [SerializeField]
    private List<int> maxRandomNumbers = new List<int>();

    [SerializeField]
    private GameObject inputButtons;

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

    public override string getDifficulty(string text, int difficulty)
    {
        string result = text;
        result = result.Replace("<min>", "1");
        result = result.Replace("<max>", (this.maxRandomNumbers[difficulty - 1]) + "");
        return result;
    }

    private void setRandomNumbers()
    {
        int max = (this.maxRandomNumbers[this.difficulty - 1] + 1);
        int start = 0;

        if (max == 4)
        {
            this.randomNumbers.Add(2);
            this.cards[0].setValue(2);
            start = 1;
        }

        for (int i = start; i < this.cards.Count; i++)
        {
            int rand = 0;
            do
            {
                rand = Random.Range(1, max);
            }
            while (this.randomNumbers.Contains(rand));
            this.randomNumbers.Add(rand);
            this.cards[i].setValue(rand);
        }
    }

    public void input(int value) //ON CLICK
    {
        this.cards[this.index].show();
        enableInputs(false);
        this.value = value;
    }

    public override void checkIfWon()
    {
        if (this.value != 0)
        {
            if ((this.randomNumbers[this.index - 1] < this.randomNumbers[this.index] && this.value == 1)
             || (this.randomNumbers[this.index - 1] > this.randomNumbers[this.index] && this.value == -1)) this.setMarkAndEndRound(true);
            else this.setMarkAndEndRound(false);
        }
        else
        {
            enableInputs(true);
        }
    }

}
