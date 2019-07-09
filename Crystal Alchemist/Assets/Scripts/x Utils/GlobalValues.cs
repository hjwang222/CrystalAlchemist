using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    public static float soundEffectVolume = 1f;
    public static float soundEffectPitch = 1f;
    public static float backgroundMusicVolume = 0.3f;

    public static Color[] red = new Color[] { new Color32(255, 0, 0, 255) , new Color32(125, 0, 0, 255) };
    public static Color[] blue = new Color[] { new Color32(0, 125, 255, 255), new Color32(0, 25, 125, 255) };
    public static Color[] green = new Color[] { new Color32(0, 255, 0, 255), new Color32(0, 125, 0, 255) };

    public static string version = "2.0 Alpha";

    public static float transitionDuration = 1f;
}
