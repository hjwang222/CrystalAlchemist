using UnityEngine;

public class CharacterCreatorGear : CharacterCreatorButton
{
    public CharacterCreatorPartProperty property;

    [SerializeField]
    private GameObject colorButton;

    [SerializeField]
    private bool removeIt = false;

    private void Start()
    {
        this.enableColorButton();
    }

    private void OnEnable()
    {
        this.enableColorButton();
    }

    public override void Click()
    {
        if(!this.removeIt) this.mainMenu.creatorPreset.AddCharacterPartData(this.property.parentName, this.property.partName);
        else this.mainMenu.creatorPreset.RemoveCharacterPartData(this.property.parentName);
               
        //if (!property.isDyeable) this.mainMenu.creatorPreset.RemoveColorGroup(property.colorGroup);
        this.enableColorButton();

        base.Click();
    }           

    private void enableColorButton()
    {
        CharacterPartData data = this.mainMenu.creatorPreset.GetCharacterPartData(this.property);
        //if (data != null && this.colorButton != null) this.colorButton.SetActive(this.property.isDyeable);
    }
    
}
