using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorGear : CharacterCreatorButton
{
    [SerializeField]
    private CharacterCreatorPartProperty property;

    public override void Click()
    {
        this.mainMenu.creatorPreset.AddCharacterPartData(this.property.parentName, this.property.partName);
        base.Click();
    }           
    
}
