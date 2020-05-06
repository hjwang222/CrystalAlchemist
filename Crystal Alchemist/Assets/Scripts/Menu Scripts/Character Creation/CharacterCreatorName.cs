using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharacterCreatorName : CharacterCreatorButton
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Selectable confirmButton;

    private float vertical;
    private PlayerInputs inputAction;

    private void Awake()
    {
        this.inputAction = new PlayerInputs();
        this.inputAction.Controls.Movement.performed += ctx => vertical = ctx.ReadValue<Vector2>().y;
        if(vertical == 0) this.inputAction.Controls.Targeting.performed += ctx => vertical = ctx.ReadValue<Vector2>().y;
    }

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
        if(this.vertical != 0) this.inputField.DeactivateInputField();
    }
}
