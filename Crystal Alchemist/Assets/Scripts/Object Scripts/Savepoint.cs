using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : Interactable
{
    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("Submit"))
        {            
            if (this.player != null)
            {
                SaveSystem.Save(this.player);
                //this.player.showDialogBox(this.text);
            }
        }
    }
}
