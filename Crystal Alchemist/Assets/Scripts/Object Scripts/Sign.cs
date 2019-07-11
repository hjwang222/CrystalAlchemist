using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    public override void doSomething()
    {
        this.player.showDialogBox(this.text);        
    }
}
