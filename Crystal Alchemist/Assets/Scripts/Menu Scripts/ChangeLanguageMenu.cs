using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguageMenu : MonoBehaviour
{
    [SerializeField]
    private Image germanFlag;

    [SerializeField]
    private Image englishFlag;

    [SerializeField]
    private SimpleSignal changeLanguageSignal;

    private void OnEnable()
    {
        getFlag();
    }

    private void getFlag()
    {
        if (GlobalValues.useAlternativeLanguage)
        {
            this.englishFlag.gameObject.SetActive(true);
            this.germanFlag.gameObject.SetActive(false);
        }
        else
        {
            this.englishFlag.gameObject.SetActive(false);
            this.germanFlag.gameObject.SetActive(true);
        }
    }

    public void changeLanguage()
    {
        if (GlobalValues.useAlternativeLanguage) GlobalValues.useAlternativeLanguage = false;
        else GlobalValues.useAlternativeLanguage = true;

        SaveSystem.SaveOptions();
        getFlag();

        this.changeLanguageSignal.Raise();
    }

    public void changeLanguageWithoutSave()
    {
        if (GlobalValues.useAlternativeLanguage) GlobalValues.useAlternativeLanguage = false;
        else GlobalValues.useAlternativeLanguage = true;

        getFlag();

        this.changeLanguageSignal.Raise();
    }
}
