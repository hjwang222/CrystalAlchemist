using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CustomUnityMenu : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Alchemist Menu/Set Cursor To Empty Buttons")]
    public static void SetCursor()
    {
        ButtonExtension[] buttons = FindObjectsOfType<ButtonExtension>(true);
        CustomCursor cursor = FindObjectOfType<CustomCursor>();

        foreach(ButtonExtension button in buttons)
        {
            if (button.GetCursor() == null) button.SetCursor(cursor);
        }
    }

    private static void SetLocalisation()
    {
        List<StatusEffect> temp = new List<StatusEffect>();
        temp.AddRange(Resources.LoadAll<StatusEffect>("Scriptable Objects/StatusEffects"));

        foreach (StatusEffect effect in temp)
        {
            //UpdateLocalisation(effect.nameValue, effect.name + "_Name", effect.statusEffectName, effect.statusEffectNameEnglish, LocalisationFileType.statuseffects);
            //UpdateLocalisation(effect.nameValue, effect.name + "_Description", effect.statusEffectDescription, effect.statusEffectDescriptionEnglish, LocalisationFileType.statuseffects);

        }

        AssetDatabase.Refresh();
        Debug.Log("Done");
    }

    /*
    private static void UpdateLocalisation(LocalisationValue value, string key, string german, string english, LocalisationFileType type)
    {
        List<string> temp = LocalisationSystem.GetLines(type);
        List<string> result = new List<string>();

        string line = string.Format("\"{0}\",\"{1}\",\"{2}\",", key, german, english);
        value.key = key;
        value.type = type;

        if (!temp.Contains(line)) LocalisationSystem.AddLine(type, line);
    }*/
    
#endif
}
