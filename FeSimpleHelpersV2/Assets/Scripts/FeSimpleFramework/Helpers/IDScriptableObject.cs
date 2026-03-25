using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FeSimpleHelpers.Core
{
	[Serializable]
	public class IDScriptableObject : ScriptableObject
	{
		[ReadOnly]
		public string metaGuid = "";
		[ReadOnly]
		public string uniqueID = "";
		public string ID => uniqueID;

		public string displayName;

		protected virtual void Awake()
		{
			Validate();
		}

		public virtual void OnEnable()
		{
			Validate();
		}

		protected virtual void Validate()
		{
#if UNITY_EDITOR
			bool hasChanged = false;

			if (Utils.ValidateUniqueID(ref uniqueID))
				hasChanged = true;

			if (string.IsNullOrEmpty(metaGuid))
			{
				metaGuid = GetGUID();
				hasChanged = true;
			}

			if (string.IsNullOrEmpty(displayName))
			{
				hasChanged = true;
				displayName = name.Replace("Stat - ", "");
			}

			if (hasChanged)
			{
				Debug.LogError("IDScriptableObject id changed by code: " + name, this);

				EditorUtility.SetDirty(this);
				SerializedObject so = new SerializedObject(this);
				so.ApplyModifiedProperties();
			}
#endif
		}

		public void ValidateDuplicatedInEditor()
		{
#if UNITY_EDITOR
			string thisMetaGUID = GetGUID();

			if (thisMetaGUID != metaGuid)
			{
				Debug.LogError("ASSET DUPLICATE IN EDITOR: " + name, this);
				Debug.LogError("duplicated uniqueID: " + uniqueID);
				uniqueID = "";
				metaGuid = thisMetaGUID;
				Validate();
				Debug.LogError("new uniqueID: " + uniqueID);
			}
#endif
		}

#if UNITY_EDITOR
		public string GetGUID()
		{
			string assetPath = AssetDatabase.GetAssetPath(this);
			string guid = AssetDatabase.AssetPathToGUID(assetPath);
			return guid;
		}
#endif

	}
}