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
    private TextMeshProUGUI previewText;

    private void Start()
    {
        this.textfield.text = "";
        this.previewText.text = "";
    }

    public void textChanged()
    {
        this.previewText.text = this.textfield.text;
        this.mainMenu.creatorPreset.characterName = this.textfield.text;
    }
}
