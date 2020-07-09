using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
	/// <summary>
	///   <para>Custom Editor for the Button Component.</para>
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Button2), true)]
	public class Button2Editor : SelectableEditor
	{
		SerializedProperty m_OnClickProperty;
		SerializedProperty m_OnClick2Property;
		SerializedProperty m_UsePointerEventDataProperty;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
			m_OnClick2Property = serializedObject.FindProperty("m_OnClick2");
			m_UsePointerEventDataProperty = serializedObject.FindProperty("m_UsePointerEventData");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();

			serializedObject.Update();

			m_UsePointerEventDataProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Use payload ?", "Use event payload associated with pointer (mouse / touch) events as a dynamic parameter ?"), m_UsePointerEventDataProperty.boolValue);

			if (m_UsePointerEventDataProperty.boolValue)
				EditorGUILayout.PropertyField(m_OnClick2Property);
			else
				EditorGUILayout.PropertyField(m_OnClickProperty);
			serializedObject.ApplyModifiedProperties();
		}
	}
}