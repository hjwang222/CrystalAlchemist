using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("Submit"))
        {
            if (this.player != null) this.player.showDialogBox(this.text);
        }
    }
}
