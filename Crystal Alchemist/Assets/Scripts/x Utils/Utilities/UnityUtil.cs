using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtil : MonoBehaviour
{
    public static void GetChildObjects<T>(Transform transform, List<T> childObjects)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<T>() != null) childObjects.Add(child.GetComponent<T>());
            GetChildObjects(child, childObjects);
        }
    }

    public static Vector2 PixelPerfectClamp(Vector2 moveVector)
    {
        float pixelsPerUnit = 16;

        Vector2 vectorInPixels = new Vector2(Mathf.RoundToInt(moveVector.x * pixelsPerUnit),
            Mathf.RoundToInt(moveVector.y * pixelsPerUnit));

        Vector2 result = vectorInPixels / pixelsPerUnit;
        if(result != moveVector) Debug.Log(moveVector + " -> " + result);
        return result;
    }

    public static GameObject hasChildWithTag(Character character, string searchTag)
    {
        GameObject result = null;

        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == searchTag)
            {
                result = character.transform.GetChild(i).gameObject;
                return result;
            }
        }

        return result;
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

    public static int ConvertLayerMask(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}
