using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorRace : CharacterCreatorButton
{
    [SerializeField]
    private Race race;

    [SerializeField]
    private CharacterCreatorSubMenu subMenu;


    public override void Click()
    {
        this.mainMenu.creatorPreset.setRace(this.race);
        this.mainMenu.updateGear();
        this.subMenu.ShobSubMenu();
        base.Click();
    }
}
