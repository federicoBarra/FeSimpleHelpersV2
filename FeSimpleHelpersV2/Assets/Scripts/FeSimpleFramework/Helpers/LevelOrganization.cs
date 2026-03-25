using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// This is expected to only be used in editor
/// </summary>
public class LevelOrganization : MonoBehaviour
{
	public List<Transform> levelFolders;

	public enum FolderCat
	{
		Floor,
		Houses,
		Trees,
		Walls,
		Chimi,
		Obstacles,
		Unreachable,
		Other
	}

	public void SendToFolder(Transform son, FolderCat cat)
	{
		int catIndex = (int)cat;
		if (levelFolders.Count > catIndex && levelFolders[catIndex]!= null)
			SetParent(son, levelFolders[catIndex]);
		else
		{
			Debug.LogError("something is wrong: " + levelFolders.Count + "-" + catIndex);
		}
	}
	public void SetParent(Transform son, Transform parent)
	{
		if (son == parent)
			Debug.LogError("WTF MAN: " + son.name + " -> " + parent.name);
		else
		{
			son.parent = parent;
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelOrganization))]
[CanEditMultipleObjects]
public class LevelOrganizationEditor : Editor
{
	LevelOrganization lookAtPoint;

	public override void OnInspectorGUI()
	{
		lookAtPoint = target as LevelOrganization;
		DrawDefaultInspector();
		EditorGUILayout.BeginVertical();
		string[] names = Enum.GetNames(typeof(LevelOrganization.FolderCat));

		for (var i = 0; i < names.Length; i++)
		{
			var VARIABLE = names[i];
			EditorGUILayout.LabelField(i + " - " + VARIABLE);
		}

		EditorGUILayout.EndVertical();
	}
}
#endif