using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CharacterCreatorMenu : MenuControls
{
    public CharacterPreset creatorPreset;

    [SerializeField]
    private CharacterPreset playerPreset;

    [SerializeField]
    private SimpleSignal presetSignal;

    [SerializeField]
    private List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();

    [Required]
    [SerializeField]
    private string firstScene = "Haus";

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

    public void Confirm()
    {
        this.setPreset(this.creatorPreset, this.playerPreset); //save Preset
        updatePreview();
    }

    public void startTheGame(string scene)
    {
        setPreset(this.creatorPreset, this.playerPreset);
        SceneManager.LoadSceneAsync(scene);
        Cursor.visible = false;
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

    public void setRandomPreset()
    {
        List<CharacterCreatorButton> children = new List<CharacterCreatorButton>();
        CustomUtilities.UnityFunctions.GetChildObjects<CharacterCreatorButton>(this.transform, children);

        foreach(CharacterCreatorButton child in children)
        {
            if (child.GetComponent<CharacterCreatorPreset>() == null)
            {
                int random = Random.Range(0, 101);
                if (random < 50) child.Click();
            }
        }
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
