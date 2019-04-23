using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : Interactable
{
    private void Update()
    {
        if (this.isPlayerInRange && Input.GetButtonDown("A-Button"))
        {
            if (this.character.GetComponent<Player>() != null) this.character.GetComponent<Player>().showDialogBox(this.text);
        }
    }
}
