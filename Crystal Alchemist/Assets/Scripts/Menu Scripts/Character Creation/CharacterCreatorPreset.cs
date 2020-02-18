using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorPreset : CharacterCreatorButton
{
    [HideIf("isRandom", true)]
    [SerializeField]
    private CharacterPreset preset;

    [HideIf("preset", null)]
    [SerializeField]
    private bool isRandom = false;

    public override void Click()
    {
        if (isRandom)
            this.mainMenu.setRandomPreset(); //buggy!
        else 
            this.mainMenu.setPreset(this.preset, this.mainMenu.creatorPreset);

        base.Click();
    }

    private bool setByRandom()
    {
        return true;
    }


}
