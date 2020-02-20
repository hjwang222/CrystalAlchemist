using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

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
    private GameObject confirmDialogBox;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();

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
        setPreset(this.playerPreset, this.creatorPreset);
        updateGear();
        updatePreview();
    }

    public void showConfirmDialog()
    {
        if (this.confirmDialogBox != null) this.confirmDialogBox.SetActive(true);
    }

    public void Confirm()
    {
        this.setPreset(this.creatorPreset, this.playerPreset); //save Preset
        updatePreview();
        exitMenu();
    }

    public void setPreset(CharacterPreset source, CharacterPreset target)
    {
        target.setRace(source.getRace());
        target.AddColorGroupRange(source.GetColorGroupRange());
        target.AddCharacterPartDataRange(source.GetCharacterPartDataRange());
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
