using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorRace : CharacterCreatorButton
{
    [SerializeField]
    private Race race;

    [SerializeField]
    private SimpleSignal characterPartSignal;

    public override void Click()
    {
        this.creatorPreset.race = this.race;
        this.characterPartSignal.Raise();

        base.Click();
    }
}
