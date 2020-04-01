using UnityEngine;

public class ChangeLayoutMenu : TitleScreenMenues
{
    private void OnEnable()
    {
        getLayout();
    }

    private void getLayout()
    {
        if (GlobalGameObjects.settings.layoutType == LayoutType.keyboard) this.switchButtons(this.secondButton, this.firstButton);
        else this.switchButtons(this.firstButton, this.secondButton);
    }

    public void changeLayout(GameObject gameObject)
    {
        if (gameObject.name.ToLower() == "keyboard") GlobalGameObjects.settings.layoutType = LayoutType.keyboard;
        else GlobalGameObjects.settings.layoutType = LayoutType.gamepad;
        
        getLayout();

        this.switchSignal.Raise();
    }
}
