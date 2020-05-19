using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Language
{
    German,
    English
}

public enum LocalisationFileType
{
    skills,
    items,
    characters,
    dialogs,
    menues,
    statuseffects,
    objects,
    minigames,
    maps
}

public class LocalisationData
{
    public Dictionary<string, string> skills;
    public Dictionary<string, string> items;
    public Dictionary<string, string> characters;
    public Dictionary<string, string> dialogs;
    public Dictionary<string, string> menues;
    public Dictionary<string, string> maps;
    public Dictionary<string, string> statuseffects;
    public Dictionary<string, string> objects;
    public Dictionary<string, string> minigames;

    private char lineSeperator = '\n';
    private char surround = '"';
    private string[] fieldSeperator = { "\",\"" };
    public Language language;

    public LocalisationData(Language language, TextAsset skillTexts, TextAsset itemTexts,   
                     TextAsset characterTexts, TextAsset dialogTexts, TextAsset menuTexts,
                     TextAsset mapTexts, TextAsset statusEffects, TextAsset objectTexts, TextAsset miniGameTexts)
    {
        this.language = language;
        string attributeID = GetLanguageID();
        skills = GetDictionaryValues(attributeID, skillTexts);
        items = GetDictionaryValues(attributeID, itemTexts);
        characters = GetDictionaryValues(attributeID, characterTexts);
        dialogs = GetDictionaryValues(attributeID, dialogTexts);
        menues = GetDictionaryValues(attributeID, menuTexts);
        maps = GetDictionaryValues(attributeID, mapTexts);
        statuseffects = GetDictionaryValues(attributeID, statusEffects);
    }

    public string GetLocalisedValue(string key, LocalisationFileType type)
    {
        string value = key;

        switch (type)
        {
            case LocalisationFileType.skills: this.skills.TryGetValue(key, out value); break;
            case LocalisationFileType.items: this.items.TryGetValue(key, out value); break;
            case LocalisationFileType.dialogs: this.dialogs.TryGetValue(key, out value); break;
            case LocalisationFileType.characters: this.characters.TryGetValue(key, out value); break;
            case LocalisationFileType.menues: this.menues.TryGetValue(key, out value); break;
            case LocalisationFileType.statuseffects: this.statuseffects.TryGetValue(key, out value); break;
            case LocalisationFileType.objects: this.objects.TryGetValue(key, out value); break;
            case LocalisationFileType.minigames: this.minigames.TryGetValue(key, out value); break;
        }

        return value;
    }

    private string GetLanguageID()
    {
        if (this.language == Language.English) return "en";
        return "de";
    }

    private Dictionary<string, string> GetDictionaryValues(string attributeID, TextAsset file)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] lines = file.text.Split(lineSeperator);

        int attributeIndex = 1;
        string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);

        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(attributeID))
            {
                attributeIndex = i;
                break;
            }
        }

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] fields = CSVParser.Split(line);

            for (int f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd(surround);
            }

            if (fields.Length > attributeIndex)
            {
                var key = fields[0];

                if (dictionary.ContainsKey(key)) { continue; }
                var value = fields[attributeIndex];
                dictionary.Add(key, value);
            }
        }

        return dictionary;
    }
}
