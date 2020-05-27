using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    public override void DoOnSubmit()
    {
        this.player.GetComponent<PlayerDialog>().showDialog(this);
    }
}
