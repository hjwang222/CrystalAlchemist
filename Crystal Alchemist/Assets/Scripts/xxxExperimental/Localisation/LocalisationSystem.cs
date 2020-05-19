using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public static class LocalisationSystem
{
    public static bool isInit;

    public static LocalisationData german;
    public static LocalisationData english;

    public static void Init()
    {
        LoadFiles();
        isInit = true;
    }

    public static string GetLocalisedValue(string key, LocalisationFileType type)
    {
        if(!isInit) Init();

        switch (MasterManager.settings.language)
        {
            case Language.English: return english.GetLocalisedValue(key, type);
            case Language.German: return german.GetLocalisedValue(key, type);
        }

        return "";
    }

    public static void LoadFiles()
    {
        TextAsset skillTexts = Resources.Load<TextAsset>("Data/Localisation/Skills");
        TextAsset itemTexts = Resources.Load<TextAsset>("Data/Localisation/Items");
        TextAsset characterTexts = Resources.Load<TextAsset>("Data/Localisation/Characters");
        TextAsset dialogTexts = Resources.Load<TextAsset>("Data/Localisation/Dialogs");
        TextAsset menuTexts = Resources.Load<TextAsset>("Data/Localisation/Menues");
        TextAsset mapTexts = Resources.Load<TextAsset>("Data/Localisation/Maps");

        german = new LocalisationData(Language.German, skillTexts, itemTexts, characterTexts, dialogTexts, menuTexts, mapTexts);
        english = new LocalisationData(Language.English, skillTexts, itemTexts, characterTexts, dialogTexts, menuTexts, mapTexts);
    }
}
