using UnityEngine;

public class ChangeLayoutMenu : OptionsSwitch
{
    private void OnEnable() => getLayout();    

    private void getLayout()
    {
        if (MasterManager.settings.layoutType == LayoutType.keyboard) this.switchButtons(this.secondButton, this.firstButton);
        else this.switchButtons(this.firstButton, this.secondButton);
    }

    public void changeLayout(GameObject gameObject)
    {
        if (gameObject.name.ToLower() == "keyboard") MasterManager.settings.layoutType = LayoutType.keyboard;
        else MasterManager.settings.layoutType = LayoutType.gamepad;
        
        getLayout();
        SettingsEvents.current.DoLayoutChange();
    }
}
