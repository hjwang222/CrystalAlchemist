using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameMachine : Rewardable
{
    [SerializeField]
    private MiniGame miniGame;

    public override void doSomethingOnSubmit()
    {
        if (Utilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price))
        {
            MiniGame miniGame = Instantiate(this.miniGame);
        }
        else
        {
            Utilities.DialogBox.showDialog(this, this.player, DialogTextTrigger.failed);
        }
    }
}
