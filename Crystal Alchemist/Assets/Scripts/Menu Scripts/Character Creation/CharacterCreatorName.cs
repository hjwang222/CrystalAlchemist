using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterCreatorName : CharacterCreatorButton
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Selectable confirmButton;

    [SerializeField]
    private StringValue playerName;

    private void Start()
    {
        if(GameEvents.current != null) GameEvents.current.OnCancel += Deactivate;
    }

    private void OnDestroy()
    {
        if (GameEvents.current != null) GameEvents.current.OnCancel -= Deactivate;
    }  

    public void Confirm()
    {
        this.playerName.SetValue(this.inputField.text);

        if (this.inputField.text.Length > 1) UnityUtil.SetInteractable(this.confirmButton, true);
        else UnityUtil.SetInteractable(this.confirmButton, false);

        Deactivate();
    }

    public void activeOnSelect() => this.inputField.ActivateInputField();
    
    private void Deactivate() => this.inputField.DeactivateInputField();    
}
