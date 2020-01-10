using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    public override void doSomethingOnSubmit()
    {
        CustomUtilities.DialogBox.showDialog(this, this.player);
    }
}
