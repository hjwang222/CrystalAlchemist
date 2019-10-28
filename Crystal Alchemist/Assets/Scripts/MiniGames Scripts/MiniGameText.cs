using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameText : MonoBehaviour
{
    [SerializeField]
    private MiniGameUI miniGameUI;

    [SerializeField]
    private bool showDialogBox = false;

    public void Disable()
    {
        if (this.showDialogBox) showDialog(); //WIN or LOSE
        else this.miniGameUI.startRound();
        this.gameObject.SetActive(false);
    }

    public void setLoot(Item item)
    {
        
    }

   public void showDialog()
    {
        this.miniGameUI.showDialog();
    }

}
