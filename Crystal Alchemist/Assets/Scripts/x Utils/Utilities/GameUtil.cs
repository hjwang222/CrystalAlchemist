using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameUtil : MonoBehaviour
{
    public static string getButton(enumButton button)
    {
        return button.ToString();
    }

    public static void setPreset(CharacterPreset source, CharacterPreset target)
    {
        target.setRace(source.getRace());
        target.characterName = source.characterName;
        target.AddColorGroupRange(source.GetColorGroupRange());
        target.AddCharacterPartDataRange(source.GetCharacterPartDataRange());
    }

    public static void ShowMenu(GameObject newActiveMenu, List<GameObject> menues)
    {
        foreach (GameObject gameObject in menues)
        {
            gameObject.SetActive(false);
        }

        if (newActiveMenu != null && menues.Count > 0)
        {
            newActiveMenu.SetActive(true);

            for (int i = 0; i < newActiveMenu.transform.childCount; i++)
            {
                ButtonExtension temp = newActiveMenu.transform.GetChild(i).GetComponent<ButtonExtension>();
                if (temp != null && temp.setFirstSelected)
                {
                    temp.setFirst();
                    break;
                }
            }
        }
    }

    public static float setResource(float resource, float max, float addResource)
    {
        if (addResource != 0)
        {
            if (resource + addResource > max) addResource = max - resource;
            else if (resource + addResource < 0) resource = 0;

            resource += addResource;
        }

        return resource;
    }
}

