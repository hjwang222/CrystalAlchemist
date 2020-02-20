using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterCreatorName : CharacterCreatorButton
{
    [SerializeField]
    private TMP_InputField textfield;



    [SerializeField]
    private Selectable confirmButton;

    private void Start()
    {
        setNameFromPreset();
        this.confirmButton.gameObject.SetActive(false);
    }

    private void setNameFromPreset()
    {
        this.textfield.text = this.mainMenu.creatorPreset.characterName;
        this.mainMenu.updatePreview();
    }

    private void OnEnable()
    {
        setNameFromPreset();
    }

    public void textChanged()
    {
        this.mainMenu.updatePreview();
        this.mainMenu.creatorPreset.characterName = this.textfield.text;
    }

    public void activeOnSelect()
    {
        this.textfield.ActivateInputField();
    }

    public void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0) this.textfield.DeactivateInputField();

        if (this.textfield.text.Length > 1) this.confirmButton.gameObject.SetActive(true);
        else this.confirmButton.gameObject.SetActive(false);
    }
}
