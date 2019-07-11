using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : Interactable
{
    public override void doSomething()
    {
        SaveSystem.Save(this.player);
        this.player.showDialogBox(this.text);
    }
}
