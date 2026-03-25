#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(UnityEngine.Object), true)]
public class ButtonAttributeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var targetObject = target;
		var type = targetObject.GetType();

		var methods = type.GetMethods(
			BindingFlags.Instance |
			BindingFlags.Static |
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.FlattenHierarchy
		);

		foreach (var method in methods)
		{
			var buttonAttributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);

			foreach (ButtonAttribute buttonAttribute in buttonAttributes)
			{
				string label = string.IsNullOrEmpty(buttonAttribute.Label)
					? ObjectNames.NicifyVariableName(method.Name)
					: buttonAttribute.Label;

				if (GUILayout.Button(label))
				{
					Undo.RecordObject(targetObject, "Button Pressed: " + method.Name);

					method.Invoke(targetObject, null);

					EditorUtility.SetDirty(targetObject);
				}
			}
		}
	}
}
#endif