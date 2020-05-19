using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Linq;

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
        TextAsset effectTexts = Resources.Load<TextAsset>("Data/Localisation/StatusEffects");
        TextAsset miniGameTexts = Resources.Load<TextAsset>("Data/Localisation/MiniGames");
        TextAsset objectTexts = Resources.Load<TextAsset>("Data/Localisation/Objects");

        german = new LocalisationData(Language.German, skillTexts, itemTexts, characterTexts, 
            dialogTexts, menuTexts, mapTexts, effectTexts, objectTexts, miniGameTexts);
        english = new LocalisationData(Language.English, skillTexts, itemTexts, characterTexts, 
            dialogTexts, menuTexts, mapTexts, effectTexts, objectTexts, miniGameTexts);
    }

    private static string GetPath(LocalisationFileType type)
    {
        string path = "Assets/Resources/Data/Localisation/";
        switch (type)
        {
            case LocalisationFileType.characters: path += "Characters"; break;
            case LocalisationFileType.dialogs: path += "Dialogs"; break;
            case LocalisationFileType.items: path += "Items"; break;
            case LocalisationFileType.maps: path += "Maps"; break;
            case LocalisationFileType.menues: path += "Menues"; break;
            case LocalisationFileType.skills: path += "Skills"; break;
            case LocalisationFileType.statuseffects: path += "StatusEffects"; break;
        }

        path += ".csv";
        return path;
    }

    public static void AddLine(LocalisationFileType type, string line)
    {
        string path = GetPath(type);
        string lines = line + Environment.NewLine;
        WriteLine(path, lines);
        
    }

    public static List<string> GetLines(LocalisationFileType type)
    {
        string path = GetPath(type);
        List<string> result = File.ReadAllLines(path).ToList();
        return result;
    }

    private static void WriteLine(string path, string line)
    {
       if (File.Exists(path)) File.AppendAllText(path, line);
    }

    private static void WriteLine(string path, string key, string ger, string eng)
    {
        string line = string.Format("\"{0}\",\"{1}\",\"{2}\",", key, ger, eng)+Environment.NewLine;
        if (File.Exists(path)) File.AppendAllText(path, line);        
    }
}
