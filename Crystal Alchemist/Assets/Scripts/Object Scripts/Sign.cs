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
            if(this.dialogScript != null) this.dialogScript.showDialog(this.character, this.text);
        }
    }
}
