using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatusBarMenu : TitleScreenMenues
{
    [SerializeField]
    private ResourceType resourceType;

    private void OnEnable()
    {
        getLayout();
    }

    private void getLayout()
    {
        if(this.resourceType == ResourceType.life) setLayout(GlobalValues.healthBar);
        else if (this.resourceType == ResourceType.mana) setLayout(GlobalValues.manaBar);
    }

    private void setLayout(bool value)
    {
        if (value) this.switchButtons(this.secondButton, this.firstButton);
        else this.switchButtons(this.firstButton, this.secondButton);
    }

    public void changeLayout(bool useBar)
    {
        if (this.resourceType == ResourceType.life) GlobalValues.healthBar = useBar;
        else if (this.resourceType == ResourceType.mana) GlobalValues.manaBar = useBar;

        getLayout();

        this.switchSignal.Raise();
    }
}
