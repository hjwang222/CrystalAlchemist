using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayoutType
{
    gamepad,
    keyboard
}

public class GlobalValues : MonoBehaviour
{
    public static float soundEffectVolume = 1f;
    public static float soundEffectPitch = 1f;
    public static float backgroundMusicVolume = 0.3f;
    public static float backgroundMusicPitch = 1f;

    public static float backgroundMusicVolumeMenu = 0.5f;

    public static LayoutType layoutType = LayoutType.gamepad;
    public static bool useAlternativeLanguage = false;
    public static bool healthBar = false;
    public static bool manaBar = false;

    public static Color[] red = new Color[] { new Color32(255, 0, 0, 255) , new Color32(125, 0, 0, 255) };
    public static Color[] blue = new Color[] { new Color32(0, 125, 255, 255), new Color32(0, 25, 125, 255) };
    public static Color[] green = new Color[] { new Color32(0, 255, 0, 255), new Color32(0, 125, 0, 255) };

    public static float playerDelay = 0.1f;

    public static float transitionDuration = 1f;

    public static string saveGameFiletype = "dat";


    public static float getMusicInMenu()
    {
        return backgroundMusicVolume * backgroundMusicVolumeMenu;
    }    
}
