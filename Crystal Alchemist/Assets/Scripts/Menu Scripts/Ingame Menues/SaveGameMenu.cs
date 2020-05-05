using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SaveGameMenu : MenuControls
{
    [BoxGroup("Save Menu")]
    [Required]
    [SerializeField]
    private List<SaveSlot> slots = new List<SaveSlot>();

    //Called from Dialogbox
    public void UpdateSaves()
    {
        foreach(SaveSlot slot in this.slots) slot.getData();       
        this.slots[0].GetComponent<ButtonExtension>().setFirst();
    }
}


