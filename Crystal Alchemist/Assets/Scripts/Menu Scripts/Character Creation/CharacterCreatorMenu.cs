using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CharacterCreatorMenu : MenuControls
{
    [BoxGroup("Character Creator")]
    [Required]
    public CharacterPreset creatorPreset;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private CharacterPreset playerPreset;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private SimpleSignal presetSignal;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();

    [BoxGroup("Character Creator")]
    [SerializeField]
    private MenuDialogBoxLauncher dialogBoxLauncherConfirm;

    public override void Start()
    {
        base.Start();
        init();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        init();
    }

    private void init()
    {
        CustomUtilities.Preset.setPreset(this.playerPreset, this.creatorPreset);
        updateGear();
        updatePreview();
    }

    public void showConfirmDialog()
    {
        if (this.dialogBoxLauncherConfirm != null) this.dialogBoxLauncherConfirm.raiseDialogBox();
    }

    public void Confirm()
    {
        CustomUtilities.Preset.setPreset(this.creatorPreset, this.playerPreset); //save Preset
        updatePreview();
        exitMenu();
    }

    public void updatePreview()
    {
        this.presetSignal.Raise();
    }

    public void updateGear()
    {
        List<CharacterCreatorGear> gearButtons = new List<CharacterCreatorGear>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorGear>(this.transform, gearButtons);

        foreach (CharacterCreatorPartProperty part in this.properties)
        {
            //enableGearButton(gearButtons, part);
            CharacterPartData data = this.creatorPreset.GetCharacterPartData(part.parentName, part.partName);
            bool enableIt = part.enableIt(this.creatorPreset.getRace(), data);

            if (enableIt) this.creatorPreset.AddCharacterPartData(part.parentName, part.partName);
            else this.creatorPreset.RemoveCharacterPartData(part.parentName, part.partName);
        }
    }
}
