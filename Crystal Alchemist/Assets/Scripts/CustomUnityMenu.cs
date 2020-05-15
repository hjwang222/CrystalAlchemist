using UnityEditor;
using UnityEngine;

public class CustomUnityMenu : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Alchemist Menu/Set Cursor To Empty Buttons")]
    public static void SetCursor()
    {
        ButtonExtension[] buttons = Object.FindObjectsOfType<ButtonExtension>(true);
        CustomCursor cursor = Object.FindObjectOfType<CustomCursor>();

        foreach(ButtonExtension button in buttons)
        {
            if (button.GetCursor() == null) button.SetCursor(cursor);
        }
    }
#endif
}
