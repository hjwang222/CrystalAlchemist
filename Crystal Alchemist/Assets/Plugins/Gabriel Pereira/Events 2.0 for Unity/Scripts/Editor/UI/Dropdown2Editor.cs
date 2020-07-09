using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
	[CustomEditor(typeof(Dropdown2), true)]
	[CanEditMultipleObjects]
	public class Dropdown2Editor : SelectableEditor
	{
		SerializedProperty m_Template;
		SerializedProperty m_CaptionText;
		SerializedProperty m_CaptionImage;
		SerializedProperty m_ItemText;
		SerializedProperty m_ItemImage;
		SerializedProperty m_Value;
		SerializedProperty m_Options;
		SerializedProperty m_CallbackType;
		SerializedProperty m_OnValueChanged;
		SerializedProperty m_OnLabelChanged;
		SerializedProperty m_OnSpriteChanged;
		SerializedProperty m_OnValueLabelChanged;
		SerializedProperty m_OnValueSpriteChanged;
		SerializedProperty m_OnLabelSpriteChanged;
		SerializedProperty m_OnValueLabelSpriteChanged;

		GUIContent[] m_CallbackTypeNames = null;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_Template = serializedObject.FindProperty("m_Template");
			m_CaptionText = serializedObject.FindProperty("m_CaptionText");
			m_CaptionImage = serializedObject.FindProperty("m_CaptionImage");
			m_ItemText = serializedObject.FindProperty("m_ItemText");
			m_ItemImage = serializedObject.FindProperty("m_ItemImage");
			m_Value = serializedObject.FindProperty("m_Value");
			m_Options = serializedObject.FindProperty("m_Options");
			m_CallbackType = serializedObject.FindProperty("m_CallbackType");
			m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");
			m_OnLabelChanged = serializedObject.FindProperty("m_OnLabelChanged");
			m_OnSpriteChanged = serializedObject.FindProperty("m_OnSpriteChanged");
			m_OnValueLabelChanged = serializedObject.FindProperty("m_OnValueLabelChanged");
			m_OnValueSpriteChanged = serializedObject.FindProperty("m_OnValueSpriteChanged");
			m_OnLabelSpriteChanged = serializedObject.FindProperty("m_OnLabelSpriteChanged");
			m_OnValueLabelSpriteChanged = serializedObject.FindProperty("m_OnValueLabelSpriteChanged");

			m_CallbackTypeNames = new GUIContent[]
			{
				new GUIContent("Index", "The index of new selected option is passed as parameter for callback functions"),
				new GUIContent("Text", "The text of new selected option is passed as parameter for callback functions"),
				new GUIContent("Image", "The image of new selected option is passed as parameter for callback functions"),
				new GUIContent("Index and Text", "The index and text of new selected option is passed as parameter for callback functions"),
				new GUIContent("Index and Image", "The index and image of new selected option is passed as parameter for callback functions"),
				new GUIContent("Text and Image", "The text and image of new selected option is passed as parameter for callback functions"),
				new GUIContent("All", "The index, text and image of new selected option is passed as parameter for callback functions"),
			};
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();

			serializedObject.Update();
			EditorGUILayout.PropertyField(m_Template);
			EditorGUILayout.PropertyField(m_CaptionText);
			EditorGUILayout.PropertyField(m_CaptionImage);
			EditorGUILayout.PropertyField(m_ItemText);
			EditorGUILayout.PropertyField(m_ItemImage);
			EditorGUILayout.PropertyField(m_Value);
			EditorGUILayout.PropertyField(m_Options);

			m_CallbackType.intValue = EditorGUILayout.Popup(new GUIContent("Callback Type", "The type of callback when dropdown value changes"), m_CallbackType.intValue, m_CallbackTypeNames);

			Dropdown2.CallbackType callbackType = (Dropdown2.CallbackType)m_CallbackType.intValue;

			switch (callbackType)
			{
				case Dropdown2.CallbackType.INDEX:
					EditorGUILayout.PropertyField(m_OnValueChanged);
					break;
				case Dropdown2.CallbackType.LABEL:
					EditorGUILayout.PropertyField(m_OnLabelChanged);
					break;
				case Dropdown2.CallbackType.SPRITE:
					EditorGUILayout.PropertyField(m_OnSpriteChanged);
					break;
				case Dropdown2.CallbackType.INDEX_LABEL:
					EditorGUILayout.PropertyField(m_OnValueLabelChanged);
					break;
				case Dropdown2.CallbackType.INDEX_SPRITE:
					EditorGUILayout.PropertyField(m_OnValueSpriteChanged);
					break;
				case Dropdown2.CallbackType.LABEL_SPRITE:
					EditorGUILayout.PropertyField(m_OnLabelSpriteChanged);
					break;
				case Dropdown2.CallbackType.ALL:
					EditorGUILayout.PropertyField(m_OnValueLabelSpriteChanged);
					break;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}