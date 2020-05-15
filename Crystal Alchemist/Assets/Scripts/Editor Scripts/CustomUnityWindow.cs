using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomUnityWindow : EditorWindow
{
    [MenuItem("Alchemist Menu/Player Animation Update")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CustomUnityWindow));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Update Player Animation"))
        {
            if (EditorUtility.DisplayDialog("Update Animation", "Do you want to update all animations?\nThis could take a while... .", "Do it", "Nope")) Debug.Log("Hallo");
        }
    }
}
