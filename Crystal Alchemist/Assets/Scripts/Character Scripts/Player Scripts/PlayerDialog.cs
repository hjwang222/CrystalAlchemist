using System.Collections.Generic;
using UnityEngine;

class PlayerDialog : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public void showDialog(Interactable interactable)
    {
        showDialog(interactable, null);
    }

    public void showDialog(Interactable interactable, DialogTextTrigger trigger)
    {
        showDialog(interactable, trigger, null);
    }

    public void showDialog(Interactable interactable, ItemStats loot)
    {
        if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.GetComponent<DialogSystem>().show(this.player, interactable, loot);
    }

    public void showDialog(Interactable interactable, DialogTextTrigger trigger, ItemStats loot)
    {
        if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.gameObject.GetComponent<DialogSystem>().show(this.player, trigger, interactable, loot);
    }
}

