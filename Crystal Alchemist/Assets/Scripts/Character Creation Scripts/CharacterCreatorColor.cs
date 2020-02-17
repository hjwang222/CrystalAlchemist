using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorColor : CharacterCreatorButton
{
    [SerializeField]
    private ColorGroup colorGroup;

    [SerializeField]
    private Image image;

    public override void Click()
    {
        this.mainMenu.creatorPreset.AddColorGroup(this.colorGroup, this.image.color);

        base.Click();
    }


}
