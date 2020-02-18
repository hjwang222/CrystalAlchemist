using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorGear : CharacterCreatorButton
{
    public CharacterCreatorPartProperty property;

    [SerializeField]
    private bool removeIt = false;

    public override void Click()
    {
        if(!this.removeIt) this.mainMenu.creatorPreset.AddCharacterPartData(this.property.parentName, this.property.partName);
        else this.mainMenu.creatorPreset.RemoveCharacterPartData(this.property.parentName);
        this.mainMenu.updateColor(this.property);
        base.Click();
    }           
    
}
