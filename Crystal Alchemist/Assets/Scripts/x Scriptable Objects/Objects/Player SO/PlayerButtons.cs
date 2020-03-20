using System;
using System.Collections.Generic;
using UnityEngine;

public enum enumButton
{
    AButton,
    BButton,
    XButton,
    YButton,
    RBButton,
    LBButton
}

[CreateAssetMenu(menuName = "Game/Player/Player Buttons")]
public class PlayerButtons : ScriptableObject
{
    public Ability AButton;
    public Ability BButton;
    public Ability XButton;
    public Ability YButton;
    public Ability LBButton;
    public Ability RBButton;
    
    public void GetAbilityFromButton(out Ability ability, out string button)
    {
        ability = null;
        button = "";

        foreach (enumButton item in Enum.GetValues(typeof(enumButton)))
        {
            if (isPressed(item.ToString()))
            {
                button = item.ToString();
                ability = GetAbilityFromButton(item);
            }
        }
    }

    private bool isPressed(string button)
    {
        if (Input.GetButton(button)
            || Input.GetButtonUp(button)
            || Input.GetButtonDown(button)) return true;

        return false;
    }

    public Ability GetAbilityFromButton(enumButton button)
    {
        switch (button)
        {
            case enumButton.AButton: return this.AButton;
            case enumButton.BButton: return this.BButton;
            case enumButton.XButton: return this.XButton;
            case enumButton.YButton: return this.YButton;
            case enumButton.RBButton: return this.RBButton;
            case enumButton.LBButton: return this.LBButton;
            default: return null;
        }
    }

    public void SetAbilityToButton(Ability ability, enumButton button)
    {
        switch (button)
        {
            case enumButton.AButton: this.AButton = ability; break;
            case enumButton.BButton: this.BButton = ability; break;
            case enumButton.XButton: this.XButton = ability; break;
            case enumButton.YButton: this.YButton = ability; break;
            case enumButton.RBButton: this.RBButton = ability; break;
            case enumButton.LBButton: this.LBButton = ability; break;
            default: break;
        }
    }

    public void SetAbilityToButton(string button, Ability ability)
    {
        switch (button)
        {
            case "A": this.AButton = ability; break;
            case "B": this.BButton = ability; break;
            case "X": this.XButton = ability; break;
            case "Y": this.YButton = ability; break;
            case "RB": this.RBButton = ability; break;
            case "LB": this.LBButton = ability; break;
            default: break;
        }
    }

    public List<string[]> saveButtonConfig()
    {
        List<string[]> result = new List<string[]>();

        if (this.AButton != null) result.Add(new string[] { "A", this.AButton.name });
        if (this.BButton != null) result.Add(new string[] { "B", this.BButton.name });
        if (this.XButton != null) result.Add(new string[] { "X", this.XButton.name });
        if (this.YButton != null) result.Add(new string[] { "Y", this.YButton.name });
        if (this.RBButton != null) result.Add(new string[] { "RB", this.RBButton.name });
        if (this.LBButton != null) result.Add(new string[] { "LB", this.LBButton.name });

        return result;
    }
}
