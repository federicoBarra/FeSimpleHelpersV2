using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FeSimpleHelpers.StatsSystem
{
	/// <summary>
	/// Basically a list of stats and their base values. This is what a character class or item would have to define their stats.
	/// The StatHandler will then pull from this and apply modifiers to get the final stat values.
	/// </summary>
	[CreateAssetMenu(menuName = "FeSimpleFramework/Stats/Base Stats Definition")]
	public class BaseStatsDefinition : ScriptableObject
	{
		[System.Serializable]
		public class StatVal
		{
			public StatConfig stat;
			public float baseValue;
		}

		public Sprite icon;
		public List<StatVal> stats;

		//TODO process this as a dictionary for performance
		//TODO check for dupes
		public float GetVal(StatConfig _statType) 
		{
			foreach (StatVal stat in stats)
			{
				if(stat.stat ==_statType)
					return stat.baseValue;
			}
			return 0;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(BaseStatsDefinition.StatVal))]
	public class StatValDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			// Remove indent
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Get properties
			SerializedProperty statProp = property.FindPropertyRelative("stat");
			SerializedProperty baseValueProp = property.FindPropertyRelative("baseValue");

			// Layout
			float spacing = 5f;
			float statWidth = position.width * 0.65f;
			float valueWidth = position.width * 0.35f - spacing;

			Rect statRect = new Rect(position.x, position.y, statWidth, position.height);
			Rect valueRect = new Rect(position.x + statWidth + spacing, position.y, valueWidth, position.height);

			EditorGUI.PropertyField(statRect, statProp, GUIContent.none);
			EditorGUI.PropertyField(valueRect, baseValueProp, GUIContent.none);

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}

	[CustomEditor(typeof(BaseStatsDefinition))]
	public class BaseStatsDefinitionEditor : Editor
	{
		SerializedProperty iconProp;
		SerializedProperty statsProp;

		void OnEnable()
		{
			iconProp = serializedObject.FindProperty("icon");
			statsProp = serializedObject.FindProperty("stats");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(iconProp);

			EditorGUILayout.Space(10);

			DrawStatsHeader();
			EditorGUILayout.PropertyField(statsProp, true);

			serializedObject.ApplyModifiedProperties();
		}

		void DrawStatsHeader()
		{
			Rect rect = EditorGUILayout.GetControlRect();

			float spacing = 5f;
			float statWidth = rect.width * 0.65f;
			float valueWidth = rect.width * 0.35f - spacing;

			Rect statRect = new Rect(rect.x, rect.y, statWidth, rect.height);
			Rect valueRect = new Rect(rect.x + statWidth + spacing, rect.y, valueWidth, rect.height);

			EditorGUI.LabelField(statRect, "Stat", EditorStyles.boldLabel);
			EditorGUI.LabelField(valueRect, "Base Value", EditorStyles.boldLabel);

			// subtle separator line
			Rect lineRect = EditorGUILayout.GetControlRect(false, 1);
			EditorGUI.DrawRect(lineRect, new Color(0.3f, 0.3f, 0.3f));
		}
	}
#endif

}