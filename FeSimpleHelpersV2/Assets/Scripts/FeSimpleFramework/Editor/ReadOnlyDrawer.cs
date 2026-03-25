#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Save the previous GUI enabled state
		bool previousGUIState = GUI.enabled;

		// Disable the GUI, making the field non-interactive
		GUI.enabled = false;

		// Draw the property field using the standard method
		EditorGUI.PropertyField(position, property, label, true);

		// Restore the previous GUI enabled state
		GUI.enabled = previousGUIState;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		// Ensures multi-line fields (like arrays or large text areas) are displayed correctly
		return EditorGUI.GetPropertyHeight(property, label, true);
	}
}
#endif