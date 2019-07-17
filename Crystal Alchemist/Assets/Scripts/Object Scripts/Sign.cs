using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    public override void doSomethingOnSubmit()
    {
        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);

        this.player.showDialogBox(text);        
    }
}
