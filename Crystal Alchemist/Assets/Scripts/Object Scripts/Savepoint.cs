using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : Interactable
{
    public override void doSomethingOnSubmit()
    {
        SaveSystem.Save(this.player);
        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);
        this.player.showDialogBox(text);
    }
}
