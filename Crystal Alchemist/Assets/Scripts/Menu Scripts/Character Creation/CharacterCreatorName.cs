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
        this.confirmButton.gameObject.SetActive(false);
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
    }

    public void activeOnSelect()
    {
        this.inputField.ActivateInputField();
    }

    public void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0) this.inputField.DeactivateInputField();

        if (this.inputField.text.Length > 1) this.confirmButton.gameObject.SetActive(true);
        else this.confirmButton.gameObject.SetActive(false);
    }
}
