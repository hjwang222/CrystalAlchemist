using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterCreatorName : CharacterCreatorButton
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Selectable confirmButton;

    private void Start()
    {        
        setNameFromPreset();
    }

    private void setNameFromPreset()
    {
        this.inputField.text = this.mainMenu.creatorPreset.characterName;
    }

    private void OnEnable()
    {
        setNameFromPreset();
    }

    public void Confirm()
    {
        this.mainMenu.creatorPreset.characterName = this.inputField.text;

        if (this.inputField.text.Length > 1) UnityUtil.SetInteractable(this.confirmButton, true);
        else UnityUtil.SetInteractable(this.confirmButton, false);

        Deactivate();
    }

    public void activeOnSelect()
    {
        this.inputField.ActivateInputField();
    }    

    private void Deactivate()
    {
        this.inputField.DeactivateInputField();
    }
}
