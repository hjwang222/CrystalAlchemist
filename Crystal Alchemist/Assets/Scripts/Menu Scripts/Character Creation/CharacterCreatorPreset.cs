using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorPreset : CharacterCreatorButton
{
    [SerializeField]
    private CharacterPreset preset;

    public override void Click()
    {
        GameUtil.setPreset(this.preset, this.mainMenu.creatorPreset);
        base.Click();
    }
}
