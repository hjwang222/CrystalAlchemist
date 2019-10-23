using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameMachine : Rewardable
{
    [SerializeField]
    private MiniGame miniGame;

    public override void doSomethingOnSubmit()
    {
        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);

        if (Utilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price, text))
        {
            MiniGame miniGame = Instantiate(this.miniGame);
            miniGame.setRewards(null);
        }
    }
}
