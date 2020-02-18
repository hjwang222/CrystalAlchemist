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
            setPreset(preset);

        base.Click();
    }

    private void setPreset(CharacterPreset preset)
    {
        this.mainMenu.creatorPreset.setRace(preset.getRace());

        this.mainMenu.creatorPreset.AddColorGroupRange(preset.GetColorGroupRange());

        this.mainMenu.creatorPreset.AddCharacterPartDataRange(preset.GetCharacterPartDataRange());

    }

    private bool setByRandom()
    {
        return true;
    }


}
