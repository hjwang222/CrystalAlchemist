using System.Collections;
using System.Collections.Generic;
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

        if (this.inputField.text.Length > 1) this.confirmButton.interactable = true;
        else this.confirmButton.interactable = false;
    }

    public void activeOnSelect()
    {
        this.inputField.ActivateInputField();
    }

    public void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0) this.inputField.DeactivateInputField();
    }
}
