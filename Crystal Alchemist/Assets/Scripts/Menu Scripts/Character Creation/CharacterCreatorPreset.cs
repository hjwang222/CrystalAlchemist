using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorPreset : CharacterCreatorButton
{
    [SerializeField]
    private CharacterPreset preset;

    public override void Click()
    {
        CustomUtilities.Preset.setPreset(this.preset, this.mainMenu.creatorPreset);
        base.Click();
    }
}
