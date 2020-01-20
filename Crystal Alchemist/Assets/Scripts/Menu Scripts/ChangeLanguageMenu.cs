using UnityEngine;

public class ChangeLanguageMenu : TitleScreenMenues
{
    private void OnEnable()
    {
        getFlag();
    }

    private void getFlag()
    {
        if (GlobalValues.useAlternativeLanguage) this.switchButtons(this.secondButton, this.firstButton);
        else this.switchButtons(this.firstButton, this.secondButton);
    }

    public void changeLanguage(GameObject gameObject)
    {
        changeLanguageWithoutSave(gameObject);
        SaveSystem.SaveOptions();
    }

    public void changeLanguageWithoutSave(GameObject gameObject)
    {
        if(gameObject.name.ToUpper() == "GER") GlobalValues.useAlternativeLanguage = false;
        else GlobalValues.useAlternativeLanguage = true;

        getFlag();

        this.switchSignal.Raise();
    }
}
