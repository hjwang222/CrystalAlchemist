using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : Interactable
{
    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("Submit"))
        {
            Player player = this.character.GetComponent<Player>();
            
            if (player != null)
            {
                SaveSystem.Save(player);
                player.showDialogBox(this.text);
            }
        }
    }
}
