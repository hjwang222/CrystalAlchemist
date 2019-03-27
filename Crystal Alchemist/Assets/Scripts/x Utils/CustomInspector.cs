using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(LootTable))]
public class CustomInspector : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 160, position.height);
        var unitRect = new Rect(position.x + 165, position.y, 180, position.height);
        var nameRect = new Rect(position.x + 325, position.y, 20, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("item"), GUIContent.none);
        //EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("dropRate"), GUIContent.none);
        EditorGUI.IntSlider(unitRect, property.FindPropertyRelative("dropRate"), 0, 100, GUIContent.none);
        EditorGUI.LabelField(nameRect, "%");
        //EditorGUI.PropertyField(nameRect, null, "%", GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

