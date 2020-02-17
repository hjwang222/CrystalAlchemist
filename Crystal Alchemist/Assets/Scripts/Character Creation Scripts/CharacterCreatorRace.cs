using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorRace : CharacterCreatorButton
{
    [SerializeField]
    private Race race;

    public override void Click()
    {
        this.mainMenu.creatorPreset.race = this.race;
        this.mainMenu.updateGear();
    }
}
