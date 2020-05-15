using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

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





    /*
    [MenuItem("Alchemist Menu/Load Into Localisation")]
    public static void SetLocal()
    {
        UITextTranslation[] translation = Object.FindObjectsOfType<UITextTranslation>(true);
        int count = 0;

        for(int i = 0; i < translation.Length; i++)
        {
            UITextTranslation trans = translation[i];
            string englishText = trans.alternativeText;
            string germanText = trans.GetComponent<TextMeshProUGUI>().text;

            string gameObject = trans.gameObject.name;
            string parent = "";
            if (trans.transform.parent != null) parent = trans.transform.parent.gameObject.name;

            string key = parent + "_" + gameObject + "_" + i;
            string line = string.Format("\n\"{0}\",\"{1}\",\"{2}\"", key, germanText, englishText);
            File.AppendAllText("Assets/Resources/Data/Localisation/Menues.csv", line, System.Text.Encoding.UTF8);
            count = i;
        }
        Debug.Log("Done " + count);
    }*/
#endif
}
