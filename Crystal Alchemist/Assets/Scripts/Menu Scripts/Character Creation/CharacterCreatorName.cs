using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class CharacterCreatorName : CharacterCreatorButton
{
    [SerializeField]
    private StringValue playerName;

    [SerializeField]
    private StringListValue values;

    private TMP_Dropdown nameDropDown;

    private void Start()
    {
        this.nameDropDown = GetComponent<TMP_Dropdown>();
        this.nameDropDown.AddOptions(values.GetValue());
        UnityUtil.SelectDropDown(this.nameDropDown, this.playerName.GetValue());
    }

    public void SetName()
    {
        this.playerName.SetValue(this.nameDropDown.captionText.text);
    }

    /*
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

        if (this.inputField.text.Length > 1) this.confirmButton.interactable = true;
        else this.confirmButton.interactable = false;

        Deactivate();
    }

    public void activeOnSelect() => this.inputField.ActivateInputField();
    
    private void Deactivate() => this.inputField.DeactivateInputField();    */

}
